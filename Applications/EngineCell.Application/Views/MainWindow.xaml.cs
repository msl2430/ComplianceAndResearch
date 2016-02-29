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
using EngineCell.Application.Models;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Core.Constants;
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

        public MainWindow()
        {
            InitializeComponent();

            ToggleLogWindow(Properties.Settings.Default.IsLogVisible);
            MainWindowViewModel = new MainWindowViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory()
            {
                ApplicationViewModel = MainWindowViewModel,
                CellPoints = (new CellPointRepository()).GetCellPointsByCellId(1) //TODO: Need to refactor this
                                .ToList()
                                .Select(p => new PointDataModel(p))
                                .ToList(),
            };
            
            MainWindowViewModel.PointConfigViewModel = new PointConfigurationViewModel(ApplicationSessionFactory);
            MainWindowViewModel.TestDisplayViewModel = new TestDisplayViewModel(ApplicationSessionFactory, new StripChartViewModel(ApplicationSessionFactory));
            MainWindowViewModel.ViewModels = new ObservableCollection<BaseViewModel>() { MainWindowViewModel.PointConfigViewModel, MainWindowViewModel.TestDisplayViewModel };
            
            //ChangePageView(MainWindowViewModel.TestDisplayViewModel);

            DataContext = MainWindowViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            PointWorkerService = new PointWorkerService(ApplicationSessionFactory);

            RemainingTimer.SetTimer(TimeSpan.FromMinutes(45));
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
                    break;
                case StatusConstants.ConnectionStatus.Connected:
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
            ChangePageView(MainWindowViewModel.TestDisplayViewModel);
        }
        #endregion

        #region Private Methods
        private void ToggleLogWindow(bool isShowWindow)
        {
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
