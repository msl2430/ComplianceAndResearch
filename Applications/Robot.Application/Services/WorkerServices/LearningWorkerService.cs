﻿using Opto22.Core.Constants;
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
    internal interface ILearningWorkerService
    {
        ThrottleSetPointChartModel TspModel { get; set; }
        bool IsRunning { get; }
        void DoWork();
        bool IsCancelPending();
        void CancelWork();
    }

    internal sealed class LearningWorkerService : BaseWorkerThreadService, ILearningWorkerService
    {
        private const int Interval = 250;
        
        private LearningViewModel ViewModel { get; set; }
        public ThrottleSetPointChartModel TspModel { get; set; }

        public LearningWorkerService(LearningViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ViewModel = viewModel;
            WaitStopWatch = new Stopwatch();
        }

        public override void DoWork()
        {
            CancellationToken = new CancellationTokenSource();
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.LearningPhaseActive.ToInt(), 1);
            while (!CancellationToken.IsCancellationRequested)
            {
                if(!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                if (WaitStopWatch.ElapsedMilliseconds > Interval)
                {
                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();
                    switch (ViewModel.LearningProgressStatus)
                    {
                        case (int)StatusConstants.PhaseStatus.PreCheckActive:
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                                ScratchPadConstants.LoadStatus.Loading.ToInt());
                            ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase completed pre-check.", true);
                            ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.LoadDataPointActive.ToInt();
                            SetTspScratchPad();
                            SetModelInformation();
                            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestPhaseDataLoadStatus.ToInt(),
                                ScratchPadConstants.LoadStatus.LoadFinished.ToInt());
                            break;
                        case (int)StatusConstants.PhaseStatus.LoadDataPointActive:
                            ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase completed loading data points.", true);
                            ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.Running.ToInt();
                            ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase running.", true);
                            break;
                        case (int)StatusConstants.PhaseStatus.Running:
                            if (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.TestPhaseActive.ToInt()).Value == 0)
                                ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.Shutdown.ToInt();
                            break;
                        case (int)StatusConstants.PhaseStatus.Shutdown:
                            ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase completed shutting down.", true);
                            CancellationToken.Cancel();
                            break;
                        default:
                            ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase initializing.", true);
                            ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.PreCheckActive.ToInt();
                            Thread.Sleep(1250); //TODO: Add precheck logic
                            break;
                    }
                }
            }
        }

        private void SetTspScratchPad()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.LearingPhaseDataLoadStatus.ToInt(),
                ScratchPadConstants.LoadStatus.Loading.ToInt());

            var setPointsByGear = TspModel.ToThrottleSetPointScratchPad();

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadTspValues(
                setPointsByGear, 
                DataConstants.SpeedPoints.ToList(),
                DataConstants.AccelerationPoints.ToList(),
                ViewModel.LearningPhaseRoadTest.RoadTestPoints
                    .OrderBy(rt => rt.SecondFromStart)
                    .Select(rt => new ScratchPadModel<decimal>(ViewModel.LearningPhaseRoadTest.RoadTestPoints.IndexOf(rt), "RoadTest Second: " + rt.SecondFromStart, rt.Speed)).ToList());
        }

        private void SetModelInformation()
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ManufacturerId.ToInt(), ViewModel.ManufacturerId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelId.ToInt(), ViewModel.ModelId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelYear.ToInt(), ViewModel.ModelYear);
        }
        
        protected override void WorkCompleted()
        {
            ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.InActive.ToInt();
        }

    }
}
