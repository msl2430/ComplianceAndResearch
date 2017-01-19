using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices.Widget
{
    public sealed class TestScheduleWidgetWorkerService : BaseWorkerThreadService
    {
        private TestScheduleDisplayViewModel ViewModel { get; set; }

        private Stopwatch TestRunTime { get; set; }

        private const int LoadInterval = 120;
        private int LastLoadIndex = 0;
        private int NextLoadIndex = 0;
        private int LastScratchpadIndex = 0;
        private decimal MaxRunTime = 0;

        public TestScheduleWidgetWorkerService(TestScheduleDisplayViewModel viewModel)
        {
            ViewModel = viewModel;
            TestRunTime = new Stopwatch();
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();

                //Check if we're connected to and collecting data from Opto before proceeding
                while (ViewModel.ApplicationSessionFactory.OptoMmpFactory == null || !ViewModel.ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                {
                    Thread.Sleep(1000);
                }

                //Check if we're running the phase for this widget
                while (ViewModel.ApplicationSessionFactory.CurrentPhaseRunning == null || ViewModel.ApplicationSessionFactory.CurrentPhaseRunning.CellTestPhaseId != ViewModel.Widget.CellTestPhaseId)
                {
                    Thread.Sleep(1000);
                }

                LoadSetpoints();
                //If we have more then double the load interval then start with double the amount of load intervals in the scratchpad - then replace half at each appropriate interval.
                if (ViewModel.CalculatedScheduleData.First().Value.Count > LoadInterval * 2)
                    LoadSetpoints();

                var timeout = Convert.ToInt64(ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleTimeout).Value);
                MaxRunTime = ViewModel.CalculatedScheduleData.Max(d => d.Value.Max(v => v.TimeIntoStage)) < timeout ? ViewModel.CalculatedScheduleData.Max(d => d.Value.Max(v => v.TimeIntoStage)) : timeout;

                //Start chart on PAC
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTestScheduleWidget.ToInt(), 1);
                if (!TestRunTime.IsRunning)
                    TestRunTime.Start();

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    if (!ViewModel.Widget.IsRunning)
                    {
                        ViewModel.Widget.IsComplete = true;
                        CancellationToken.Cancel();
                        continue;
                    }
                    
                    if (TestRunTime.ElapsedMilliseconds / 1000L >= Convert.ToInt64(ViewModel.CalculatedScheduleData.First().Value.Max(v => v.TimeIntoStage)))
                    {
                        //Test done
                        ViewModel.Widget.IsComplete = true;
                        CancellationToken.Cancel();
                        continue;
                    }

                    if (TestRunTime.ElapsedMilliseconds / 1000 >= Convert.ToInt64(MaxRunTime))
                    {
                        //Phase end setting run time met - exit with error
                        ViewModel.Widget.IsError = true;
                        ViewModel.Widget.ErrorReason = "Phase end setting MAX RUN TIME met. (" + MaxRunTime + " seconds)";
                        CancellationToken.Cancel();
                        continue;
                    }

                    //if there is still more data to load check if we hit the time in stage where we should load the next set of data
                    if (NextLoadIndex - LoadInterval > 0 && ViewModel.CalculatedScheduleData.First().Value.Count > NextLoadIndex - LoadInterval)
                    {
                        if (TestRunTime.ElapsedMilliseconds/1000L > Convert.ToInt64(ViewModel.CalculatedScheduleData.First().Value[NextLoadIndex - LoadInterval].TimeIntoStage))
                        {
                            LoadSetpoints();
                        }
                    }

                    ViewModel.DynoSetpoint = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.DynoSetpointValue.ToInt());
                    ViewModel.ThrottleSetpoint = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.ThrottleSetpointValue.ToInt());

                    ViewModel.TestRunTime = TestRunTime.Elapsed.Hours.ToString("0#") + ":" + TestRunTime.Elapsed.Minutes.ToString("0#") + ":" + TestRunTime.Elapsed.Seconds.ToString("0#");
                }
                WorkCompleted();
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Erorr in Test Schedule Widget: " + ex.Message, true);
            }
        }

        private void LoadSetpoints()
        {
            var scratchpadIndex = LastScratchpadIndex + LoadInterval > LoadInterval * 2 ? 0 : LastScratchpadIndex;
            var maxLoad = ViewModel.CalculatedScheduleData.First().Value.Count > NextLoadIndex + LoadInterval ? NextLoadIndex + LoadInterval : ViewModel.CalculatedScheduleData.First().Value.Count;
            foreach (var setpointType in ViewModel.CalculatedScheduleData)
            {
                switch (setpointType.Key)
                {
                    case WidgetConstants.TestScheduleSetpointType.Dyno:
                        ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.IsUsingDynoPid.ToInt(), 1);
                        var dynoScIndex = scratchpadIndex;
                        for (var i = NextLoadIndex; i < maxLoad; i++)
                        {
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.TimeInStage.ToInt() + dynoScIndex, setpointType.Value[i].TimeIntoStage);
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.DynoSetpoint.ToInt() + dynoScIndex, setpointType.Value[i].Value);
                            dynoScIndex++;
                        }
                        break;
                    case WidgetConstants.TestScheduleSetpointType.Throttle:
                        ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.IsUsingThrottle.ToInt(), 1);
                        var throttleScIndex = scratchpadIndex;
                        for (var i = NextLoadIndex; i < maxLoad; i++)
                        {                            
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.ThrottleSetpoint.ToInt() + throttleScIndex, setpointType.Value[i].Value);
                            throttleScIndex++;
                        }
                        break;
                }
            }
            NextLoadIndex = maxLoad;
            LastScratchpadIndex = scratchpadIndex+LoadInterval;
        }

        protected override void WorkCompleted()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTestScheduleWidget.ToInt(), 0);
        }
    }
}
