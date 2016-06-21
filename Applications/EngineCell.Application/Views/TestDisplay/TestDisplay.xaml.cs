using System.Threading.Tasks;
using System.Windows;
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

            if(PointWorkerService == null)
                PointWorkerService = new PointWorkerService(TestDisplayViewModel, Dispatcher);
        }

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestDisplayViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
            {
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("WARNING: Cannot start if not connected to Opto!", true);
                return;
            }
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Starting phase.", true);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);            

            var cellTestId = CellPointRepository.CreateCellTest(1);
            TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest = CellPointRepository.GetCellTestById(cellTestId);
            RunTimeClock.IsRunning = true;
            TestDisplayViewModel.PhaseStarted = true;
            Task.Run(() =>
            {
                TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Starting point data collection.", true);
                PointWorkerService.DoWork();
            });
            TestDisplayViewModel.ChartViewModel.IsPlay = true;
        }

        private void StopPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Stopping phase after running for " + RunTimeClock.RunTimeViewModel.RunTime  + " (user requested).", true);
            TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 0);
            CellPointRepository.UpdateCellTestEndTime(TestDisplayViewModel.ApplicationSessionFactory.CurrentCellTest);
            RunTimeClock.IsRunning = false;
            TestDisplayViewModel.PhaseStarted = false;
            PointWorkerService.CancelWork();
            TestDisplayViewModel.ChartViewModel.IsPlay = false;
        }
    }
}
