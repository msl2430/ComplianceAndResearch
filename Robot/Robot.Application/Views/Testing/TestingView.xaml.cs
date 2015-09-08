using System;
using System.Windows;
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

        }
    }
}
