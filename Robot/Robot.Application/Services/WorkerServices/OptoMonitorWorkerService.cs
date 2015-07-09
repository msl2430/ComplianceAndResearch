using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Opto22.Core.Constants;
using Robot.Application.Factories;
using Robot.Application.ViewModels;
using Robot.Core.Extensions;

namespace Robot.Application.Services.WorkerServices
{
    public interface IOptoMonitorWorkerService
    {
        OptoMonitorViewModel OptoMonitorViewModel { get; set; }
        bool IsRunning { get; }
        void DoWork();
        bool IsCancelPending();
        void CancelWork();
    }

    public sealed class OptoMonitorWorkerService : BaseWorkerThreadService, IOptoMonitorWorkerService
    {
        public OptoMonitorViewModel OptoMonitorViewModel { get; set; }
        
        private Stopwatch WaitStopWatch { get; set; }

        public OptoMonitorWorkerService(OptoMonitorViewModel optoMonitorViewModel)
        {
            OptoMonitorViewModel = optoMonitorViewModel;
            WaitStopWatch = new Stopwatch();
        }

        public override void DoWork()
        {
            try
            {
                var appSession = OptoMonitorViewModel.ApplicationSessionFactory;
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 500)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        if (appSession.OptoMmpFactory != null && appSession.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            GetOptoValues();
                        }
                    }
                    Thread.Sleep(500);
                }
            }
            catch (TaskCanceledException ex)
            {
                
            }
        }

        protected override void WorkCompleted()
        {
            
        }

        private void GetOptoValues()
        {
            var appSession = OptoMonitorViewModel.ApplicationSessionFactory;
            OptoMonitorViewModel.IsApplicationConnected =
                                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ConnectedToOpto.ToInt()).Value == 1;
            OptoMonitorViewModel.IsGoToLearningPhase =
                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.GoToLearningPhase.ToInt()).Value == 1;
            OptoMonitorViewModel.IsGoToTestPhase = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.GoToTestPhase.ToInt()).Value ==
                                                   1;
            OptoMonitorViewModel.TemplateLoadStatus =
                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.LearingPhaseTemplateLoadStatus.ToInt()).Value;
            OptoMonitorViewModel.SelectedManufacturerId =
                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ManufacturerId.ToInt()).Value;
            OptoMonitorViewModel.SelectedModelId = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ModelId.ToInt()).Value;
            OptoMonitorViewModel.SelectedModelYear = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ModelYear.ToInt()).Value;
            OptoMonitorViewModel.StrategyLocation = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.StrategyLocationValue.ToInt()).Value;
        }
    }
}
