using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Repositories;
using MahApps.Metro.Controls;
using Remotion.Linq.Collections;

namespace EngineCell.Application.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindowView
    {
        private MainWindowViewModel MainWindowViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = new MainWindowViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory()
            {
                ApplicationViewModel = MainWindowViewModel,                
            };

            //TODO: Remove - for testing
            var cellTestId = 1;
            ApplicationSessionFactory.CurrentCellTest = (new CellPointRepository()).GetCellTestById(cellTestId);

            ApplicationSessionFactory.CellPoints = (new CellPointRepository()).GetCellPointsByCellId(ApplicationSessionFactory.CurrentCellId) //TODO: Need to refactor this
                .ToList()
                .Select(p => new PointDataModel(p))
                .ToList();
            
            MainWindowViewModel.PointConfigViewModel = new PointConfigurationViewModel(ApplicationSessionFactory);
            MainWindowViewModel.TestDisplayViewModel = new TestDisplayViewModel(ApplicationSessionFactory, new StripChartViewModel(ApplicationSessionFactory));
            MainWindowViewModel.TestDisplayViewModel.PointWorkerService = new PointWorkerService(MainWindowViewModel.TestDisplayViewModel, Dispatcher);
            //MainWindowViewModel.WidgetConfigViewModel = new WidgetConfigViewModel(ApplicationSessionFactory);
            MainWindowViewModel.PhaseConfigViewModel = new PhaseConfigViewModel(ApplicationSessionFactory);
            MainWindowViewModel.ViewModels = new ObservableCollection<BaseViewModel>() { MainWindowViewModel.PointConfigViewModel, MainWindowViewModel.TestDisplayViewModel, MainWindowViewModel.PhaseConfigViewModel };                       

            ChangePageView(MainWindowViewModel.PhaseConfigViewModel);
            ToggleWidgets();

            DataContext = MainWindowViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            PointWorkerService = new PointWorkerService(MainWindowViewModel.TestDisplayViewModel, Dispatcher);

            ToggleLogWindow(Properties.Settings.Default.IsLogVisible);
        }

        public void ChangePageView(BaseViewModel viewModel)
        {
            foreach (var model in MainWindowViewModel.ViewModels)
            {
                model.ZIndex = 0;
            }

            MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == viewModel.GetType()).ZIndex = 1;
        }

        private void RefreshOptoConnection(bool isConnected)
        {
            //if (MainWindowViewModel.CurrentPageViewModel == null) return;

            //if (MainWindowViewModel.CurrentPageViewModel.GetType() != typeof(CarSelectionViewModel)) return;

            //var viewModel = (CarSelectionViewModel)MainWindowViewModel.CurrentPageViewModel;
            //viewModel.IsOptoConnected = isConnected;
        }

        private void ToggleOptoConnection()
        {
            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Disconnected:
                    Task.Run(() =>
                    {
                        ApplicationSessionFactory.LogEvent("Connecting to Opto 22 @" + ConfigurationManager.AppSettings["OptoIpAddress"], true);
                        RefreshOptoConnection(true);
                        OptoConnectionWorker.DoWork();
                    }).ConfigureAwait(false);
                    Task.Run(() =>
                    {
                        MainWindowViewModel.TestDisplayViewModel.PointWorkerService.DoWork();                        
                    }).ConfigureAwait(false);
                    
                    break;
                case StatusConstants.ConnectionStatus.Connected:
                    MainWindowViewModel.TestDisplayViewModel.PointWorkerService.CancelWork();
                    ApplicationSessionFactory.LogEvent("Disconnecting from Opto 22", true);                    
                    OptoConnectionWorker.CancelWork();
                    RefreshOptoConnection(false);                    
                    break;
            }
        }

        private void OptoConnectionToggle_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            ToggleOptoConnection();
        }

        private void ApplicationView_OnClosing(object sender, CancelEventArgs e)
        {
            if (ApplicationSessionFactory.OptoConnectionStatus == StatusConstants.ConnectionStatus.Connected)
                OptoConnectionWorker.CancelWork();

            while (ApplicationSessionFactory.OptoConnectionStatus == StatusConstants.ConnectionStatus.Connected)
            {
                Thread.Sleep(250);
            }
        }

        #region Events
        public void ShowHideStatusBar_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            ToggleLogWindow(LogWindow.Visibility == Visibility.Collapsed);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuAlarmConfig_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            AlarmConfiguration = (Flyout)parentWindow.FindName("AlarmConfiguration");
            AlarmConfiguration.Tag = ControlConstants.ChangeTracking.Pristine;
            AlarmConfiguration.IsOpen = true;
        }

        private void MenuPointConfig_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePageView(MainWindowViewModel.PointConfigViewModel);
        }

        private void MenuTestDisplay_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.TestDisplayViewModel.UpdateVisibleCellPoints();
            ToggleWidgets();
            ChangePageView(MainWindowViewModel.TestDisplayViewModel);
        }

        [Obsolete("Remove", false)]
        private void ToggleWidgets()
        {
            var ventCtrl1Settings = WidgetRepository.GetWidgetSettingByWidgetCell(ApplicationSessionFactory.CurrentCellId, WidgetConstants.Widget.VentilationControl1);
            if (ventCtrl1Settings.IsNotNullOrEmpty())
            {
                var settings = new VentilationControlViewModel();
                settings.SetValues(ventCtrl1Settings);

                MainWindowViewModel.TestDisplayViewModel.VentControl1Display = new VentilationControlDisplayViewModel("Ventilation Control 1", settings.IsActive);                
            }

            var dynoctrlSettings = WidgetRepository.GetWidgetSettingByWidgetCell(ApplicationSessionFactory.CurrentCellId, WidgetConstants.Widget.DynoPid);
            if (dynoctrlSettings.IsNotNullOrEmpty())
            {
                var settings = new DynoPidConfigViewModel();
                settings.SetValues(dynoctrlSettings);

                MainWindowViewModel.TestDisplayViewModel.DynoPidDisplay = new DynoPidDisplayViewModel("Dyno Pid", settings.IsActive);
            }

            var startSettings = WidgetRepository.GetWidgetSettingByWidgetCell(ApplicationSessionFactory.CurrentCellId, WidgetConstants.Widget.Starter);
            if (startSettings.IsNotNullOrEmpty())
            {
                var settings = new StarterConfigViewModel();
                settings.SetValues(startSettings);

                MainWindowViewModel.TestDisplayViewModel.StarterDisplay = new StarterDisplayViewModel("Starter", settings.IsActive);
            }
            //MainWindowViewModel.TestDisplayViewModel.VentControl1Display.IsActive =
            //    ventCtrl1Settings.Any(v => v.WidgetId == WidgetConstants.Widget.VentilationControl1 && v.WidgetSettingId == WidgetConstants.WidgetSetting.VentCtrl1Active && v.Value == "1");
        }

        private void MenuWidgetConfig_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePageView(MainWindowViewModel.PhaseConfigViewModel);
        }
        #endregion

        #region Private Methods
        private void ToggleLogWindow(bool isShowWindow)
        {
            ApplicationSessionFactory.IsLogWindowVisible = isShowWindow;
            Properties.Settings.Default.IsLogVisible = isShowWindow;
            Properties.Settings.Default.Save();
            if (isShowWindow)
            {
                HideStatusBarButton.ToolTip = "Hide Log Window";
                LogWindow.Visibility = Visibility.Visible;
                HideStatusBarButtonIcon.Visual = (Visual)FindResource("appbar_chevron_down");
                return;
            }

            HideStatusBarButton.ToolTip = "Show Log Window";
            LogWindow.Visibility = Visibility.Collapsed;
            HideStatusBarButtonIcon.Visual = (Visual)FindResource("appbar_chevron_up");
        }
        #endregion        
    }
}
