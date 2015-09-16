using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Opto22.Core.Models;
using Robot.Application.ViewModels;
using Robot.Models.Helpers;
using Robot.Models.Models;

namespace Robot.Application.Views.Testing
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class TestingView : BaseUserControl
    {
        private TestingViewModel ViewModel { get; set; }
        private ThrottleSetPointChartModel TspModel { get; set; }

        public TestingView()
        {
            InitializeComponent();
        }

        private void TestingView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (TestingViewModel) DataContext;

            TspModel = SetPointService.GetSetPointChartByModelId(ViewModel.ModelId);
            if (TspModel == null)
                throw new Exception("No Throttle Set Point Chart found for Model.");

            ViewModel.RoadTestCharts = RoadTestService.GetAllRoadTests();

            var test = ThrottleSetPointHelper.GetAllThrottleSetPoints(TspModel);

            
            SetTspScratchPad();


            for (var i = 0; i < TspModel.SetPoints.Count(sp => sp.Gear == 1); i++)
            {
                var point = TspModel.SetPoints.OrderBy(sp => sp.Speed).ThenBy(sp => sp.Acceleration).ElementAt(i);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(i, point.SetPoint);
            }
        }

        private void SetTspScratchPad()
        {
            var setPointsByGear = new List<List<IScratchPadModel<decimal>>>();
            var speedPoints = new decimal[] { 0, 10, 20, 30, 40, 50, 60};
            var accelerationPoints = new decimal[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            
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
                    //speedPointIndex = i%speedPoints.Length == 0 && i > 0 ? speedPointIndex + 1 : speedPointIndex;
                    //accelerationPointIndex = i%accelerationPoints.Length == 0 ? 0 : accelerationPointIndex + 1;

                    var point = TspModel.SetPoints.Where(sp => sp.Gear == gearNumber).OrderBy(sp => sp.Speed).ThenBy(sp => sp.Acceleration).ElementAt(i);
                    setPointList.Add(new ScratchPadModel<decimal>(i, "Gear: " + gearNumber + " Speed: " + speedPoints[speedPointIndex] + " Acceleration: " + accelerationPoints[accelerationPointIndex], point.SetPoint));
                    accelerationPointIndex++;
                }
                setPointsByGear.Add(setPointList);
            }

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadTspValues(setPointsByGear, speedPoints.ToList(), accelerationPoints.ToList());
        }
    }
}
