using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices.Widget;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for CustomChartDisplay.xaml
    /// </summary>
    public partial class CustomChartDisplay : UserControl
    {
        private CustomChartDisplayViewModel ViewModel { get; set; }

        private CustomChartWidgetWorkerService WorkerService { get; set; }

        public CustomChartDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            widget.IsRunning = true;
            ViewModel = new CustomChartDisplayViewModel(appSession, widget);
            WorkerService = new CustomChartWidgetWorkerService(ViewModel);
        }

        private void CustomChartDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            Task.Run(() =>
            {
                WorkerService.DoWork();
            });
        }
    }
}
