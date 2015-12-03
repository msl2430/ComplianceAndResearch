using Opto22.Core.Constants;
using Robot.Application.ViewModels;
using Robot.Core.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Opto22.Core.Models;
using Robot.Application.Extensions;
using Robot.Core.Constants;
using Robot.Models.Models;

namespace Robot.Application.Services.WorkerServices
{
    public sealed class SimulatorWorkerThreadService : BaseWorkerThreadService
    {
        private const int Interval = 100;

        private SimulatorViewModel ViewModel { get; set; }
        public ThrottleSetPointChartModel TspModel { get; set; }

        public SimulatorWorkerThreadService(SimulatorViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ViewModel = viewModel;
            WaitStopWatch = new Stopwatch();
        }

        public override void DoWork()
        {
            CancellationToken = new CancellationTokenSource();
            ViewModel.ApplicationSessionFactory.LogEvent("Simulator initializing.", true);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.SimulatorActive.ToInt(), 1);
            while (!CancellationToken.IsCancellationRequested)
            {
                if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                if (WaitStopWatch.ElapsedMilliseconds > Interval)
                {
                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();
                    
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

        protected override void WorkCompleted()
        {
            
        }
    }
}
