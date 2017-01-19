using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices.Widget;
using EngineCell.Application.ViewModels;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for DynoPidRampDisplay.xaml
    /// </summary>
    public partial class DynoPidRampDisplay : UserControl
    {
        private DynoPidRampDisplayViewModel ViewModel { get; set; }

        private DynoPidRampWidgetWorkerService WorkerService { get; set; }

        public DynoPidRampDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            widget.IsRunning = true;
            ViewModel = new DynoPidRampDisplayViewModel(appSession, widget);
            WorkerService = new DynoPidRampWidgetWorkerService(ViewModel);
            
        }

        private void DynoPidRampDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            if (ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.DynoRampSetpoint) || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.DynoRampTime))
            {
                MessageBox.Show("Dyno Ramp widget not properly configured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task.Run(() =>
            {
                WorkerService.DoWork();
            });
        }
    }
}
