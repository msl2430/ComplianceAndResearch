using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Views.TestDisplay
{
    /// <summary>
    /// Interaction logic for TestDisplay.xaml
    /// </summary>
    public partial class TestDisplay : BaseUserControl
    {
        private TestDisplayViewModel TestDisplayViewModel { get; set; }
        private IPointWorkerService PointWorkerService { get; set; }
        
        public TestDisplay()
        {
            InitializeComponent();
        }

        private void TestDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                TestDisplayViewModel = (TestDisplayViewModel) DataContext;

            if (PointWorkerService == null)
                PointWorkerService = new PointWorkerService(TestDisplayViewModel, Dispatcher);           
        }

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestDisplayViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
            {
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("WARNING: Cannot start if not connected to Opto!", true);
                return;
            }
            if (!TestDisplayViewModel.IsManualTest)
            {
                var runTimeModel = new ManualTimeConfigViewModel();
                var runTime = new ManualTimeConfig(runTimeModel);
                runTime.ShowDialog();
                if (runTimeModel.IsCancel)
                    return;

                TimeRemaining.RemainingTime = new TimeSpan(runTimeModel.Hour, runTimeModel.Minute, runTimeModel.Second);
                TimeRemaining.IsRunning = true;
                TimeRemaining.RemainingTimeViewModel.IsVisible = true;
            }
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Starting phase.", true);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);            

            var cellTestId = CellPointRepository.CreateCellTest(1, !TestDisplayViewModel.IsManualTest ? ControlConstants.CellTestType.Manual : ControlConstants.CellTestType.Timed);
            TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest = CellPointRepository.GetCellTestById(cellTestId);
            RunTimeClock.IsRunning = true;
            TestDisplayViewModel.PhaseStarted = true;
            Task.Run(() =>
            {
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Starting point data collection.", true);
                PointWorkerService.DoWork();
            });

            if (!TestDisplayViewModel.IsManualTest)
            {
                Task.Run(() => {
                    var worker = new TestRunnerWorkerService(TimeRemaining, this, Dispatcher);
                    worker.DoWork();
                });
            }
           
            TestDisplayViewModel.ChartViewModel.IsPlay = true;
        }

        private void StopPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            StopTest(true);
        }

        private void ToggleTestType(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            TestDisplayViewModel.IsManualTest = btn.Tag.ToString() == "manual";
        }

        public void StopTest(bool isUserRequested)
        {
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Stopping phase after running for " + RunTimeClock.RunTimeViewModel.RunTime + (isUserRequested ? " (user requested)." : " (test complete)."), true);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 0);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 0);
            CellPointRepository.UpdateCellTestEndTime(TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest);
            RunTimeClock.IsRunning = false;
            TimeRemaining.IsRunning = false;
            TestDisplayViewModel.PhaseStarted = false;
            PointWorkerService.CancelWork();
            TestDisplayViewModel.ChartViewModel.IsPlay = false;

            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Preparing results file.", true);
            var fileName = (new ExportService()).WriteCsvExport(TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest.CellTestId);
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Results file saved to: " + fileName, true);
        }
    }   
}
