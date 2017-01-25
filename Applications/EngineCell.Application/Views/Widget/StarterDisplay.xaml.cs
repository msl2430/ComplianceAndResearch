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
    /// Interaction logic for StarterDisplay.xaml
    /// </summary>
    public partial class StarterDisplay : UserControl
    {
        private StarterDisplayViewModel ViewModel { get; set; }

        private StarterWidgetWorkerService WorkerService { get; set; }

        public StarterDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            widget.IsRunning = true;
            ViewModel = new StarterDisplayViewModel(appSession, widget);
            WorkerService = new StarterWidgetWorkerService(ViewModel);
        }

        private void StarterDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            if (ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.StarterCrankTime) 
                || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.StarterNumberOfTries)
                || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.StarterStartRpm)
                || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.StarterTimeAtRpm)
                || ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.StarterTimeBetweenTries))
            {
                MessageBox.Show("Starter widget not properly configured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task.Run(() =>
            {
                WorkerService.DoWork();
            });
        }
    }
}
