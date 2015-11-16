using Opto22.Core.Constants;
using Robot.Application.Services.WorkerServices;
using Robot.Application.ViewModels;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Models.Models;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Robot.Application.Views.Testing
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class TestingView : BaseUserControl
    {
        private TestingViewModel ViewModel { get; set; }
        private ThrottleSetPointChartModel TspModel { get; set; }
        private IRoadTestWorkerService RoadTestWorkerService { get; set; }

        public TestingView()
        {
            InitializeComponent();
        }

        private void TestingView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (TestingViewModel) DataContext;

            
            ViewModel.RoadTestCharts = RoadTestService.GetAllRoadTests();
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.GoToTestPhase.ToInt(), 1);

            ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.InActive.ToInt();
            
        }

        private void StartRoadTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartRoadTestButton.IsEnabled = false;
            Task.Run(() =>
            {
                var tspModel = SetPointService.GetSetPointChartByModelId(ViewModel.ModelId);
                if (tspModel == null)
                    throw new Exception("No Throttle Set Point Chart found for Model.");
                ViewModel.ApplicationSessionFactory.LogEvent("Starting Road Test", true);
                RoadTestWorkerService = new RoadTestWorkerService(ViewModel, Dispatcher) {TspModel = tspModel};
                RoadTestWorkerService.DoWork();
            }).ContinueWith((x) =>
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Road Test completed", true);
                ViewModel.TestProgressStatus = StatusConstants.PhaseStatus.InActive.ToInt();
                Dispatcher.Invoke(() => StartRoadTestButton.IsEnabled = true);
            }).ConfigureAwait(false);
        }
    }
}
