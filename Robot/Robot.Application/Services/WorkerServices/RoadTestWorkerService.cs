using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Opto22.Core.Constants;
using Opto22.Core.Models;
using Robot.Application.Factories;
using Robot.Application.ViewModels;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Models.Models;

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
        private int _interval = 250;
        private Stopwatch WaitStopWatch { get; set; }

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
                if (WaitStopWatch.ElapsedMilliseconds > _interval)
                {
                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();
                    switch (ViewModel.TestProgressStatus)
                    {
                        case (int)StatusConstants.RoadTestStatus.PreCheckActive:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed pre-check.", true);
                            ViewModel.TestProgressStatus = StatusConstants.RoadTestStatus.LoadDataPointActive.ToInt();
                            SetTspScratchPad();
                            break;
                        case (int)StatusConstants.RoadTestStatus.LoadDataPointActive:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed loading data points.", true);
                            ViewModel.TestProgressStatus = StatusConstants.RoadTestStatus.Running.ToInt();
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test running.", true);
                            break;
                        case (int)StatusConstants.RoadTestStatus.Running:
                            if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.TestPhaseActive.ToInt()).Value == 0)
                                ViewModel.TestProgressStatus = StatusConstants.RoadTestStatus.Shutdown.ToInt();                           
                            break;
                        case (int)StatusConstants.RoadTestStatus.Shutdown:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed shutting down.", true);
                            CancellationToken.Cancel();
                            break;
                        default:
                            ViewModel.ApplicationSessionFactory.LogEvent("Road Test initializing.", true);
                            ViewModel.TestProgressStatus = StatusConstants.RoadTestStatus.PreCheckActive.ToInt();
                            Thread.Sleep(1250); //TODO: Add precheck logic
                            break;
                    }
                }
            }
        }

        private void SetTspScratchPad()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                       ScratchPadConstants.LoadStatus.Loading.ToInt());

            var setPointsByGear = new List<List<IScratchPadModel<decimal>>>();
            var speedPoints = new decimal[] { 0, 10, 20, 30, 40, 50, 60 };
            var accelerationPoints = new decimal[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var gearNumber in TspModel.SetPoints.Select(sp => sp.Gear).Distinct().OrderBy(x => x))
            {
                var setPointList = new List<IScratchPadModel<decimal>>();
                var speedPointIndex = 0;
                var accelerationPointIndex = 0;
                for (var i = 0; i < TspModel.SetPoints.Count(sp => sp.Gear == gearNumber); i++)
                {
                    if (i > 1 && i % accelerationPoints.Length == 0)
                    {
                        speedPointIndex++;
                        accelerationPointIndex = 0;
                    }

                    var point = TspModel.SetPoints.Where(sp => sp.Gear == gearNumber).OrderBy(sp => sp.Speed).ThenBy(sp => sp.Acceleration).ElementAt(i);
                    setPointList.Add(new ScratchPadModel<decimal>(i, "Gear: " + gearNumber + " Speed: " + speedPoints[speedPointIndex] + " Acceleration: " + accelerationPoints[accelerationPointIndex], point.SetPoint));
                    accelerationPointIndex++;
                }
                setPointsByGear.Add(setPointList);
            }

            var roadTestScratchPad =
                ViewModel.RoadTestCharts.First(rt => rt.RoadTestChartId == 2).RoadTestPoints.OrderBy(rt => rt.SecondFromStart).Select(
                    rt => new ScratchPadModel<decimal>(ViewModel.RoadTestCharts[0].RoadTestPoints.IndexOf(rt), "RoadTest Second: " + rt.SecondFromStart, rt.Speed));

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadTspValues(setPointsByGear, speedPoints.ToList(), accelerationPoints.ToList(), roadTestScratchPad.ToList());


            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                    ScratchPadConstants.LoadStatus.LoadFinished.ToInt());

        }

        protected override void WorkCompleted()
        {
            ViewModel.TestProgressStatus = StatusConstants.RoadTestStatus.InActive.ToInt();
        }
    }
}
