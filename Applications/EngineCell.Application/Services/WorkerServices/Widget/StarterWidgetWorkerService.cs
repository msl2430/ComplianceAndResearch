using System;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices.Widget
{
    public sealed class StarterWidgetWorkerService : BaseWorkerThreadService
    {
        public StarterDisplayViewModel ViewModel { get; set; }

        public StarterWidgetWorkerService(StarterDisplayViewModel viewModel)
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

                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.StarterStartRpm.ToInt(), ViewModel.StartRpm);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.StarterCrankTime.ToInt(), ViewModel.CrankTime);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.StarterRetryCount.ToInt(), Convert.ToDecimal(ViewModel.RetryCount));
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.StarterRetryWaitTime.ToInt(), ViewModel.TimeBetweenRetries);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.StarterCutoffTime.ToInt(), ViewModel.CutoffTime);

                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartStarterWidget.ToInt(), 1);

                while (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StarterWidgetRunning.ToInt()) == 0)
                {
                    Thread.Sleep(1000);
                }

                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StarterWidgetRunning.ToInt()) == 0)
                    {
                        //widget done
                        CancellationToken.Cancel();
                        continue;
                    }

                    ViewModel.EngineRpm = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.StarterEngineRpm.ToInt());
                    ViewModel.Attempt = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StarterAttempt.ToInt());
                    ViewModel.IsCranking = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StarterCrankOn.ToInt()) == 1;
                }

                WorkCompleted();
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Erorr in Starter Widget: " + ex.Message, true);
            }
        }

        protected override void WorkCompleted()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartStarterWidget.ToInt(), 0);
            var isEngineRunning = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StarterEngineOn.ToInt()) == 1;
            ViewModel.Widget.IsComplete = isEngineRunning;
            ViewModel.Widget.IsError = !isEngineRunning;
            if (!isEngineRunning)
                ViewModel.Widget.ErrorReason = "Engine failed to starter after " + ViewModel.RetryCount + " attempts.";
        }
    }
}
