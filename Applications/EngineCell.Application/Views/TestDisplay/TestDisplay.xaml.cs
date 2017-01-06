using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.Views.Widget;
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
        private TestDisplayViewModel ViewModel { get; set; }

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
                ViewModel = (TestDisplayViewModel) DataContext;

            if(ViewModel.Phases.IsNotNullOrEmpty())
                PrepareTestPhaseDisplay();
        }

        private void PrepareTestPhaseDisplay()
        {
            ViewModel.Phases.First().IsActive = true;
            foreach (var widget in ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases.First(p => p.CellTestPhaseId == ViewModel.Phases.First().CellTestPhaseId).Widgets)
            {
                switch (widget.WidgetId)
                {
                    case WidgetConstants.Widget.TestSchedule:
                        WidgetPanel.Children.Add(new TestScheduleDisplay(ViewModel.ApplicationSessionFactory, widget));
                        break;
                    case WidgetConstants.Widget.VentilationControl1:
                        break;
                    case WidgetConstants.Widget.VentilationControl2:
                        break;
                    case WidgetConstants.Widget.DynoPid:
                        break;
                    case WidgetConstants.Widget.Starter:
                        break;
                }
            }
        }

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            //if (ViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
            //{
            //    ViewModel.ApplicationSessionFactory.LogEvent("WARNING: Cannot start if not connected to Opto!", true);
            //    return;
            //}
            //if (!ViewModel.IsManualTest)
            //{
            //    var runTimeModel = new ManualTimeConfigViewModel();
            //    var runTime = new ManualTimeConfig(runTimeModel);
            //    runTime.ShowDialog();
            //    if (runTimeModel.IsCancel)
            //        return;

            //    TimeRemaining.RemainingTime = new TimeSpan(runTimeModel.Hour, runTimeModel.Minute, runTimeModel.Second);
            //    TimeRemaining.IsRunning = true;
            //    TimeRemaining.RemainingTimeViewModel.IsVisible = true;
            //}
            //ViewModel.ApplicationSessionFactory.LogEvent("Starting phase.", true);
            //ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);
            
            //var cellTestId = CellPointRepository.CreateCellTest(1, !ViewModel.IsManualTest ? ControlConstants.CellTestType.Manual : ControlConstants.CellTestType.Timed);
            //ViewModel.ApplicationSessionFactory.CurrentCellTest = CellPointRepository.GetCellTestById(cellTestId);
            //ExportService.CreateDataFile(ViewModel.ApplicationSessionFactory.CurrentCellTest.CellTestId, ViewModel.ApplicationSessionFactory.CellPoints.Where(cp => cp.IsRecord).ToList());
            //RunTimeClock.IsRunning = true;
            //ViewModel.ApplicationSessionFactory.LogEvent("Starting point data collection.", true);
            //ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 1);


            //if (!ViewModel.IsManualTest)
            //{
            //    Task.Run(() =>
            //    {
            //        var worker = new TestRunnerWorkerService(TimeRemaining, this, Dispatcher);
            //        worker.DoWork();
            //    });
            //}

            //ViewModel.ChartViewModel.IsPlay = true;
        }

        private void StopPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            StopTest(true);
        }

        private void ToggleTestType(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            ViewModel.IsManualTest = btn.Tag.ToString() == "manual";
        }

        public void StopTest(bool isUserRequested)
        {
            //ViewModel.ApplicationSessionFactory.LogEvent("Stopping phase after running for " + RunTimeClock.RunTimeViewModel.RunTime + (isUserRequested ? " (user requested)." : " (test complete)."), true);
            //ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 0);
            //ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 0);
            //CellPointRepository.UpdateCellTestEndTime(ViewModel.ApplicationSessionFactory.CurrentCellTest);
            //RunTimeClock.IsRunning = false;
            //TimeRemaining.IsRunning = false;
            //ViewModel.PhaseStarted = false;
            //ViewModel.ChartViewModel.IsPlay = false;

            ////ViewModel.ApplicationSessionFactory.LogEvent("Preparing results file.", true);
            //var fileName = ExportService.GetFilename();
            //if(string.IsNullOrEmpty(fileName))
            //    ViewModel.ApplicationSessionFactory.LogEvent("No data to export.", true);
            //else
            //    ViewModel.ApplicationSessionFactory.LogEvent("Results file saved to: " + fileName, true);
        }
    }   
}
