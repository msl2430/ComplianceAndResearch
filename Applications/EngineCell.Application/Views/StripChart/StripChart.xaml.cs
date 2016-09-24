using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.StripChart;

namespace EngineCell.Application.Views.StripChart
{
    /// <summary>
    /// Interaction logic for StripChart.xaml
    /// </summary>
    public partial class StripChart : UserControl
    {
        private StripChartWorkerService StripChartWorker { get; set; }
        
        public  StripChartViewModel ChartViewModel { get; set; }

        public string ChartTitle { get; set; }

        public StripChart()
        {
            InitializeComponent();                        
        }

        private void StripChart_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ChartViewModel = (StripChartViewModel) DataContext;

            if (StripChartWorker != null && StripChartWorker.IsRunning)
                return;

            ChartViewModel.CreateSeries();
            StripChartWorker = new StripChartWorkerService(ChartViewModel, Dispatcher);
            Task.Run(() => StripChartWorker.DoWork()).ConfigureAwait(false);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel.IsPlay = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            ChartViewModel.IsPlay = false;
        }       
    }
}

