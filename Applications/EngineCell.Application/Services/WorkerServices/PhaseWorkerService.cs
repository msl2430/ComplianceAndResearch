using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.Views.TestDisplay;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.Services.WorkerServices
{
    public sealed class PhaseWorkerService : BaseWorkerThreadService
    {
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        private int phaseIndex = 0;
        private TestDisplay TestDisplay { get; }
        private Dispatcher Dispatcher { get; set; }

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

                //Configure points with triggers in this phase
                foreach (var point in ApplicationSessionFactory.CellPoints)
                {
                    point.HasPhaseTrigger = ApplicationSessionFactory.CurrentPhaseRunning.Triggers.Any(t => t.CellPointId == point.CellPointId);
                }
                Thread.Sleep(1000); //wait for data to come in after setting points to collect data if they weren't already active

                while (!CancellationToken.IsCancellationRequested)
                {
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
