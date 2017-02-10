using System;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices.Widget
{
    public sealed class DynoPidRampWidgetWorkerService : BaseWorkerThreadService
    {
        private DynoPidRampDisplayViewModel ViewModel { get; set; }

        public DynoPidRampWidgetWorkerService(DynoPidRampDisplayViewModel viewModel)
        {
            ViewModel = viewModel;
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

                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.DynoPidRampSetpoint.ToInt(), ViewModel.Setpoint);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.DynoPidRampTime.ToInt(), Convert.ToDecimal(ViewModel.Time));
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.DynoPidWidgetMode.ToInt(), ViewModel.PidMode.ToInt());

                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDynoPidWidget.ToInt(), 1);

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.DynoPidWidgetRunning.ToInt()) == 0)
                    {
                        //widget done
                        ViewModel.Widget.IsComplete = true;
                        CancellationToken.Cancel();
                        continue;
                    }

                    ViewModel.CurrentSetpoint = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.DynoPidRampCurrentSetpoint.ToInt());
                    var timespan = new TimeSpan(0,0,0, Convert.ToInt32(ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.DynoPidRampCurrentTime.ToInt())));
                    ViewModel.CurrentTime = timespan.Hours.ToString("0#") + ":" + timespan.Minutes.ToString("0#") + ":" + timespan.Seconds.ToString("0#");
                }

                WorkCompleted();
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Erorr in Dyno PID Ramp Widget: " + ex.Message, true);
            }
        }

        protected override void WorkCompleted()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDynoPidWidget.ToInt(), 0);
        }
    }
}
