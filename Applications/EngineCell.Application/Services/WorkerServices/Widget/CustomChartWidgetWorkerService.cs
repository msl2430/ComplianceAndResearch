using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices.Widget
{
    public sealed class CustomChartWidgetWorkerService : BaseWorkerThreadService
    {
        private CustomChartDisplayViewModel ViewModel { get; set; }
        private Stopwatch RunTime { get; }
        public CustomChartWidgetWorkerService(CustomChartDisplayViewModel viewModel)
        {
            ViewModel = viewModel;
            RunTime = new Stopwatch();
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

                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartCustomChart1.ToInt(), 1);
                if (!RunTime.IsRunning)
                    RunTime.Start();

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    ViewModel.CurrentTime = RunTime.Elapsed.Hours.ToString("0#") + ":" + RunTime.Elapsed.Minutes.ToString("0#") + ":" + RunTime.Elapsed.Seconds.ToString("0#");

                    if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.IsCustomChart1Running.ToInt()) == 1) continue;

                    RunTime.Stop();
                    ViewModel.Widget.IsComplete = true;
                    CancellationToken.Cancel();
                }

                WorkCompleted();
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Erorr in " + ViewModel.ChartName + " Widget: " + ex.Message, true);
            }
        }

        protected override void WorkCompleted()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartCustomChart1.ToInt(), 0);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.IsCustomChart1Running.ToInt(), 0);
        }
    }
}
