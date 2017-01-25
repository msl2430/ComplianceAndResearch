using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices.Widget;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for ThrottlePidRampDisplay.xaml
    /// </summary>
    public partial class ThrottlePidRampDisplay : UserControl
    {
        private ThrottlePidRampDisplayViewModel ViewModel { get; set; }

        private ThrottlePidRampWidgetWorkerService WorkerService { get; set; }

        public ThrottlePidRampDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            widget.IsRunning = true;
            ViewModel = new ThrottlePidRampDisplayViewModel(appSession, widget);
            WorkerService = new ThrottlePidRampWidgetWorkerService(ViewModel);

        }

        private void ThrottlePidRampDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            if (ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.ThrottleRampSetpoint) || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.ThrottleRampTime))
            {
                MessageBox.Show("Throttle Ramp widget not properly configured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task.Run(() =>
            {
                WorkerService.DoWork();
            });
        }
    }
}
