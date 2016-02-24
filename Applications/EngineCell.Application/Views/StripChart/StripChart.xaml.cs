using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.StripChart;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

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

            //ChartViewModel = new StripChartViewModel();
            ChartViewModel.UpdateSeries();

            //DataContext = ChartViewModel;
            StripChartWorker = new StripChartWorkerService(ChartViewModel);

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

