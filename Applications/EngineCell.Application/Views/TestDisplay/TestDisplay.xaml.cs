using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.Services.WorkerServices;
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
        public TestDisplayViewModel ViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        private ExportService _exportService { get; set; }
        private ExportService ExportService => _exportService ?? (_exportService = new ExportService());

        private PhaseWorkerService PhaseWorkerService { get; set; }

        public TestDisplay()
        {
            InitializeComponent();
        }

        private void TestDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (TestDisplayViewModel) DataContext;            
        }

        public void PreparePhaseWidgetDisplay()
        {
            WidgetPanel.Children.Clear();

            if (ViewModel.Phases.All(p => !p.IsRunning))
                return;

            foreach (var widget in ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases.First(p => p.CellTestPhaseId == ViewModel.Phases.First(vm => vm.IsRunning).CellTestPhaseId).Widgets)
            {
                switch (widget.WidgetId)
                {
                    case WidgetConstants.Widget.TestSchedule:
                        WidgetPanel.Children.Add(new TestScheduleDisplay(ViewModel.ApplicationSessionFactory, widget));                        
                        break;
                    case WidgetConstants.Widget.DynoRamp:
                        WidgetPanel.Children.Add(new DynoPidRampDisplay(ViewModel.ApplicationSessionFactory, widget));
                        break;
                    case WidgetConstants.Widget.CustomChart1:
                    case WidgetConstants.Widget.CustomChart2:
                    case WidgetConstants.Widget.CustomChart3:
                    case WidgetConstants.Widget.CustomChart4:
                    case WidgetConstants.Widget.CustomChart5:
                        WidgetPanel.Children.Add(new CustomChartDisplay(ViewModel.ApplicationSessionFactory, widget));
                        break;
                    case WidgetConstants.Widget.Starter:
                        WidgetPanel.Children.Add(new StarterDisplay(ViewModel.ApplicationSessionFactory, widget));
                        break;
                    case WidgetConstants.Widget.Timer:
                        WidgetPanel.Children.Add(new TimerDisplay(ViewModel.ApplicationSessionFactory, widget));
                        break;
                }
            }
        }

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("WARNING: Cannot start if not connected to Opto!", true);
                return;
            }

            WidgetPanel.Children.Clear();

            if (ViewModel.Phases.IsNotNullOrEmpty())
                PreparePhaseWidgetDisplay();

            ViewModel.ApplicationSessionFactory.LogEvent("Starting phase.", true);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);
            RunTimeClock.IsRunning = true;

            PhaseWorkerService = new PhaseWorkerService(ViewModel.ApplicationSessionFactory, this, Dispatcher);
            Task.Run(() =>
            {
                PhaseWorkerService.DoWork();
            });

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 1);
            ViewModel.IsTestRunning = true;

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
            //


            //if (!ViewModel.IsManualTest)
            //{
            //    Task.Run(() =>
            //    {
            //        var worker = new TestRunnerWorkerService(TimeRemaining, this, Dispatcher);
            //        worker.DoWork();
            //    });
            //}

            ViewModel.ChartViewModel.IsPlay = true;
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
            ViewModel.ApplicationSessionFactory.LogEvent("Stopping phase after running for " + RunTimeClock.RunTimeViewModel.RunTime + (isUserRequested ? " (user requested)." : "."), true);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 0);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.TestRunning.ToInt(), 0);

            PhaseWorkerService.CancelWork();

            CellPointRepository.UpdateCellTestTime(ViewModel.ApplicationSessionFactory.CurrentCellTest, false);

            RunTimeClock.IsRunning = false;
            ViewModel.ChartViewModel.IsPlay = false;
            ViewModel.IsTestRunning = false;

            WidgetPanel.Children.Clear();



            ////ViewModel.ApplicationSessionFactory.LogEvent("Preparing results file.", true);
            //var fileName = ExportService.GetFilename();
            //if(string.IsNullOrEmpty(fileName))
            //    ViewModel.ApplicationSessionFactory.LogEvent("No data to export.", true);
            //else
            //    ViewModel.ApplicationSessionFactory.LogEvent("Results file saved to: " + fileName, true);
        }
    }   
}
