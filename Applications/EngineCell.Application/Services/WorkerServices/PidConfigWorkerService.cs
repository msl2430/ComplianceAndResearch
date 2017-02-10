using System;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IPidConfigWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public class PidConfigWorkerService : BaseWorkerThreadService, IPidConfigWorkerService
    {
        private PidConfigViewModel ViewModel { get; set; }

        public PidConfigWorkerService(PidConfigViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if(!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.IsHardwareSafety.ToInt()) == 1)
                        {
                            CancellationToken.Cancel();
                            continue;
                        }

                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();

                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (ViewModel.ApplicationSessionFactory.OptoMmpFactory == null || !ViewModel.ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }

                        ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigType.ToInt(), ViewModel.PidType.ToInt());
                        ViewModel.Input = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.InputValue.ToInt());
                        ViewModel.Output = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.OutputValue.ToInt());
                        if (!ViewModel.IsSetPointDirty)
                            ViewModel.SetPoint = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.SetPointValue.ToInt());
                    }
                    Thread.Sleep(250);
                }
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void WorkCompleted()
        {
        }
    }
}
