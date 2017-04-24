using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.Views.TestDisplay;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.Services.WorkerServices
{
    public sealed class PhaseWorkerService : BaseWorkerThreadService
    {
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        private int phaseIndex = 0;
        private TestDisplay TestDisplay { get; }
        private Dispatcher Dispatcher { get; set; }

        private DateTime LastRecordTime { get; set; }
        private int MinRecordSeconds { get; set; }
        private DateTime CaptureTime { get; set; }

        public PhaseWorkerService(IApplicationSessionFactory appSession, TestDisplay testDisplay, Dispatcher dispatcher)
        {
            ApplicationSessionFactory = appSession;
            TestDisplay = testDisplay;
            Dispatcher = dispatcher;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                ApplicationSessionFactory.LogEvent("Test starting.", true);
                SetNextPhase();
                ApplicationSessionFactory.CurrentCellTest.IsRunning = true;
                LastRecordTime = DateTime.UtcNow;

                //Configure points with triggers in this phase
                foreach (var point in ApplicationSessionFactory.CellPoints)
                {
                    point.HasPhaseTrigger = ApplicationSessionFactory.CurrentPhaseRunning.Triggers.Any(t => t.CellPointId == point.CellPointId);
                    point.IsCustomValueSet = false;
                }
                foreach (var widget in ApplicationSessionFactory.CurrentCellTest.Phases.SelectMany(p => p.Widgets))
                {
                    widget.IsRunning = false;
                    widget.IsComplete = false;
                    widget.IsError = false;
                    widget.ErrorReason = "";
                }

                Thread.Sleep(1000); //wait for data to come in after setting points to collect data if they weren't already active

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.IsHardwareSafety.ToInt()) == 1)
                    {
                        ApplicationSessionFactory.CurrentCellTest.IsRunning = false;
                        foreach (var widget in ApplicationSessionFactory.CurrentPhaseRunning.Widgets)
                        {
                            TestDisplay.ViewModel.Phases[phaseIndex - 1].IsRunning = false;
                            TestDisplay.ViewModel.Phases[phaseIndex - 1].EndDateTime = DateTime.Now;
                            widget.IsRunning = false;
                        }
                        CancellationToken.Cancel();
                        continue;
                    }

                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10 || !ApplicationSessionFactory.CurrentCellTest.IsRunning) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    if (ApplicationSessionFactory.CurrentPhaseRunning.Widgets.All(w => w.IsComplete))
                    {
                        ApplicationSessionFactory.LogEvent("Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]] complete.", true);
                        if (ApplicationSessionFactory.CurrentCellTest.Phases.Count >= phaseIndex + 1)
                        {
                            SetNextPhase();
                        }
                        else
                        {
                            //test complete
                            StopTest("Test complete.");
                            continue;
                        }
                    }

                    if (ApplicationSessionFactory.CurrentPhaseRunning.Widgets.Any(w => w.IsError))
                    {

                        foreach (var widget in ApplicationSessionFactory.CurrentPhaseRunning.Widgets.Where(w => w.IsError))
                        {
                            ApplicationSessionFactory.LogEvent(" - " + widget.ErrorReason, true);
                        }
                        ApplicationSessionFactory.LogEvent("Errors in Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]]:", true);
                        StopTest("Test cancelled.");                        
                    }

                    foreach (var trigger in ApplicationSessionFactory.CurrentPhaseRunning.Triggers)
                    {
                        var point = ApplicationSessionFactory.CellPoints.FirstOrDefault(p => p.CellPointId == trigger.CellPointId);
                        if (point == null)
                            continue;

                        if (point.Data <= trigger.LowValue)
                        {
                            TriggerActiveAction(trigger, true);
                            break;
                        }

                        if (point.Data < trigger.HighValue) continue;

                        TriggerActiveAction(trigger, false);
                        break;
                    }

                    MinRecordSeconds = ApplicationSessionFactory.CellPoints.Where(c => c.IsRecord).Min(c => c.AverageSeconds) ?? 1;
                    CaptureTime = DateTime.Now;
                    if (ApplicationSessionFactory.CurrentCellTest != null && ApplicationSessionFactory.TestStartTime != null
                        && ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt()) == 1
                        && ApplicationSessionFactory.CellPoints.Any(cp => cp.IsRecord && cp.IsActive)
                        && Math.Abs((DateTime.UtcNow - LastRecordTime).Seconds) >= MinRecordSeconds)
                    {
                        LastRecordTime = DateTime.UtcNow;
                        ExportService.WriteDataToFile(ApplicationSessionFactory.CurrentCellTest.CellTestId, CaptureTime, ApplicationSessionFactory.TestStartTime.Value, ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord && cp.IsActive).ToList());
                        //Task.Run(() =>
                        //{
                        //    CellPointRepository.CreateCellPointData(
                        //        ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord && cp.IsActive)
                        //            .Select(cp => cp.ToCellTestPointDataModel(ApplicationSessionFactory.CurrentCellTest.CellTestId, DateTime.Now))
                        //            .ToList());
                        //});
                    }
                    Thread.Sleep(100);
                }
                WorkCompleted();
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ApplicationSessionFactory.LogEvent("Erorr in Phase worker: " + ex.Message, true);
                WorkCompleted();
            }
        }

        private void TriggerActiveAction(CellTestPhaseTriggerModel trigger, bool isLowThreshold)
        {
            ApplicationSessionFactory.LogEvent("Trigger for point [[" + trigger.CellPointName + "]] active. (Value below " + (isLowThreshold ? "low" : "high") + " threshold of " + (isLowThreshold ? trigger.LowValue : trigger.HighValue) + ").", true);
            switch (trigger.ResultTypeId)
            {
                case WidgetConstants.TriggerResultType.GoToPhase:
                    if (trigger.ResultTypeId == WidgetConstants.TriggerResultType.GoToPhase && !string.IsNullOrEmpty(trigger.ResultTypeParameter))
                    {
                        ApplicationSessionFactory.LogEvent("Trigger defined to move to another phase.", true);
                        var tempPhase = TestDisplay.ViewModel.Phases.FirstOrDefault(p => p.CellTestPhaseId.ToString() == trigger.ResultTypeParameter);
                        SetNextPhase(TestDisplay.ViewModel.Phases.IndexOf(tempPhase));
                    }
                    else
                        StopTest("Error: No result parameter set for trigger! Stopping test.");
                    break;
                case WidgetConstants.TriggerResultType.Shutdown:
                    StopTest("Trigger defined to stop test. (Shutdown)");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StopTest(string logMsg)
        {
            ApplicationSessionFactory.LogEvent(logMsg, true);
            ApplicationSessionFactory.CurrentCellTest.IsRunning = false;
            foreach (var widget in ApplicationSessionFactory.CurrentPhaseRunning.Widgets)
            {
                TestDisplay.ViewModel.Phases[phaseIndex-1].IsRunning = false;
                TestDisplay.ViewModel.Phases[phaseIndex-1].EndDateTime = DateTime.Now;
                widget.IsRunning = false;
            }
            CancellationToken.Cancel();
        }

        private void SetNextPhase(int? forcedPhaseIndex = null)
        {
            var tempPhase = TestDisplay.ViewModel.Phases[forcedPhaseIndex ?? phaseIndex];
            tempPhase.IsRunning = true;
            tempPhase.StartDateTime = DateTime.Now;
            tempPhase.EndDateTime = null;
            ApplicationSessionFactory.CurrentPhaseRunning = ApplicationSessionFactory.CurrentCellTest.Phases.FirstOrDefault(p => p.CellTestPhaseId == tempPhase.CellTestPhaseId);
            foreach (var widget in ApplicationSessionFactory.CurrentPhaseRunning.Widgets)
            {
                widget.IsRunning = false;
            }
            tempPhase = null;
            ApplicationSessionFactory.LogEvent("Starting Phase [[" + ApplicationSessionFactory.CurrentPhaseRunning.Name + "]].", true);

            if (phaseIndex > 0) //finish last phase
            {
                TestDisplay.ViewModel.Phases[phaseIndex - 1].EndDateTime = DateTime.Now;
                TestDisplay.ViewModel.Phases[phaseIndex - 1].IsRunning = false;
            }

            phaseIndex++;
            Dispatcher.Invoke(() => {
                TestDisplay.PreparePhaseWidgetDisplay();
            });
        }

        protected override void WorkCompleted()
        {
            Dispatcher.Invoke(() => {
                TestDisplay.StopTest(false);
            });
        }

        public new void CancelWork()
        {
            StopTest("Stopping phases.");
        }
    }
}
