using System;
using System.Diagnostics;
using System.IO;
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
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        if (appSession.OptoMmpFactory != null && appSession.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            GetOptoValues();
                        }
                    }
                    Thread.Sleep(10);
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
            OptoMonitorViewModel.IsGoToTestPhase = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.GoToTestPhase.ToInt()).Value == 1;
            OptoMonitorViewModel.TemplateLoadStatus =
                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.LearingPhaseDataLoadStatus.ToInt()).Value;
            OptoMonitorViewModel.SelectedManufacturerId =
                appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ManufacturerId.ToInt()).Value;
            OptoMonitorViewModel.SelectedModelId = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ModelId.ToInt()).Value;
            OptoMonitorViewModel.SelectedModelYear = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.ModelYear.ToInt()).Value;
            OptoMonitorViewModel.StrategyLocation = appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.StrategyLocationValue.ToInt()).Value;

            OptoMonitorViewModel.TestPhaseCurrentSpeed = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TestPhaseCurrentSpeed.ToInt()).Value;
            OptoMonitorViewModel.TestPhaseCurrentAcceleration = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TestPhaseCurrentAcceleration.ToInt()).Value;
            OptoMonitorViewModel.TestPhaseRequiredSpeed = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TestPhaseRequiredSpeed.ToInt()).Value;
            OptoMonitorViewModel.TestPhaseThrottleSetPoint = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TestPhaseThrottleSetPoint.ToInt()).Value;
            OptoMonitorViewModel.TestPhaseCurrentRunTime = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TestPhaseCurrentRunTime.ToInt()).Value;

            if (!OptoMonitorViewModel.IsGoToTestPhase) return;
            using (var file = new StreamWriter(System.Environment.CurrentDirectory + @"\TestData.txt", true))
            {
                file.WriteLine(DateTime.Now + " >> RunTime: " + OptoMonitorViewModel.TestPhaseCurrentRunTime + " Speed: " + OptoMonitorViewModel.TestPhaseCurrentSpeed +
                               " Acceleration: " + OptoMonitorViewModel.TestPhaseCurrentAcceleration + " Required Speed: " + OptoMonitorViewModel.TestPhaseRequiredSpeed +
                               " Throttle Set Point: " + OptoMonitorViewModel.TestPhaseThrottleSetPoint);
            }
        }
    }
}
