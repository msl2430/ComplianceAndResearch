using Opto22.Core.Constants;
using Opto22.Core.Models;
using Robot.Application.Extensions;
using Robot.Application.ViewModels;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Models.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace Robot.Application.Services.WorkerServices
{
    internal interface IRoadTestWorkerService
    {
        ThrottleSetPointChartModel TspModel { get; set; }
        bool IsRunning { get; }
        void DoWork();
        bool IsCancelPending();
        void CancelWork();
    }

    internal sealed class RoadTestWorkerService : BaseWorkerThreadService, IRoadTestWorkerService
    {
        private const int Interval = 250;
        
        private TestingViewModel ViewModel { get; set; }
        public ThrottleSetPointChartModel TspModel { get; set; }

        public RoadTestWorkerService(TestingViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ViewModel = viewModel;
            WaitStopWatch = new Stopwatch();            
        }

        public override void DoWork()
        {
            CancellationToken = new CancellationTokenSource();
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseActive.ToInt(), 1);
            while (!CancellationToken.IsCancellationRequested)
            {
                if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                if (WaitStopWatch.ElapsedMilliseconds > Interval)
                {
                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();
                    switch (ViewModel.TestProgressStatus)
                    {
                        case (int)StatusConstants.PhaseStatus.PreCheckActive:
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                                ScratchPadConstants.LoadStatus.Loading.ToInt());
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed pre-check.", true);
                            ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.LoadDataPointActive.ToInt();
                            SetTspScratchPad();
                            SetModelInformation();
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                                ScratchPadConstants.LoadStatus.LoadFinished.ToInt());
                            break;
                        case (int)StatusConstants.PhaseStatus.LoadDataPointActive:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed loading data points.", true);
                            ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.Running.ToInt();
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test running.", true);
                            break;
                        case (int)StatusConstants.PhaseStatus.Running:
                            if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.TestPhaseActive.ToInt()).Value == 0)
                                ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.Shutdown.ToInt();                           
                            break;
                        case (int)StatusConstants.PhaseStatus.Shutdown:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed shutting down.", true);
                            CancellationToken.Cancel();
                            break;
                        default:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test initializing.", true);
                            ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.PreCheckActive.ToInt();
                            Thread.Sleep(1250); //TODO: Add precheck logic
                            break;
                    }
                }
            }
        }

        private void SetTspScratchPad()
        {
            var setPointsByGear = TspModel.ToThrottleSetPointScratchPad();

            var roadTestScratchPad =
                ViewModel.RoadTestCharts.First(rt => rt.RoadTestChartId == 1).RoadTestPoints.OrderBy(rt => rt.SecondFromStart).Select(
                    rt => new ScratchPadModel<decimal>(ViewModel.RoadTestCharts[0].RoadTestPoints.IndexOf(rt), "RoadTest Second: " + rt.SecondFromStart, rt.Speed)); //TODO: needs to be user selected

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadTspValues(setPointsByGear, DataConstants.SpeedPoints.ToList(), DataConstants.AccelerationPoints.ToList(), roadTestScratchPad.ToList());
        }

        private void SetModelInformation()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ManufacturerId.ToInt(), ViewModel.ManufacturerId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelId.ToInt(), ViewModel.ModelId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelYear.ToInt(), ViewModel.ModelYear);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.ModelPulseMultplier.ToInt(), ViewModel.PulseMultiplier);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.ModelPulsePerRev.ToInt(), ViewModel.PulsePerRev);
            foreach (var gearRatio in ViewModel.ModelGearRatios.OrderBy(gr => gr.Gear))
            {
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.GearRatio1.ToInt()+ViewModel.ModelGearRatios.IndexOf(gearRatio), gearRatio.Ratio);
            }
        }

        protected override void WorkCompleted()
        {
            ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.InActive.ToInt();
        }
    }
}
