using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.TestDisplay
{
    /// <summary>
    /// Interaction logic for TestDisplay.xaml
    /// </summary>
    public partial class TestDisplay : BaseUserControl
    {
        private TestDisplayViewModel TestDisplayViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        private ExportService _exportService { get; set; }
        private ExportService ExportService => _exportService ?? (_exportService = new ExportService());

        public TestDisplay()
        {
            InitializeComponent();
        }

        private void TestDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                TestDisplayViewModel = (TestDisplayViewModel) DataContext;
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
            ExportService.CreateDataFile(TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest.CellTestId, TestDisplayViewModel.ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord).ToList());
            RunTimeClock.IsRunning = true;
            TestDisplayViewModel.PhaseStarted = true;
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Starting point data collection.", true);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 1);


            if (!TestDisplayViewModel.IsManualTest)
            {
                Task.Run(() =>
                {
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
            TestDisplayViewModel.ChartViewModel.IsPlay = false;

            //TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Preparing results file.", true);
            var fileName = ExportService.GetFilename();
            if(string.IsNullOrEmpty(fileName))
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("No data to export.", true);
            else
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Results file saved to: " + fileName, true);
        }
    }   
}
