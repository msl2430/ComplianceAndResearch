using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.Views.Pid;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Views.TestDisplay
{
    /// <summary>
    /// Interaction logic for TestDisplay.xaml
    /// </summary>
    public partial class TestDisplay : UserControl
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
                PointWorkerService = new PointWorkerService(TestDisplayViewModel.ApplicationSessionFactory);
        }

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            //TestDisplayViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);
            //Task.Run(() =>
            //{
            //    TestDisplayViewModel.ApplicationSessionFactory.LogEvent("Collecting Point data.", true);
            //    PointWorkerService.DoWork();
            //});
            //TestDisplayViewModel.ChartViewModel.IsPlay = true;
            if (TestDisplayViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
                return;

            
        }
    }
}
