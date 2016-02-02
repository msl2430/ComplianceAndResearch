using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using MahApps.Metro.Controls;

namespace EngineCell.Application.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindowView
    {
        public MainWindow()
        {
            InitializeComponent();

            ToggleLogWindow(Properties.Settings.Default.IsLogVisible);
            ApplicationViewModel = new ApplicationViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory() { ApplicationViewModel = ApplicationViewModel };
            
            DataContext = ApplicationViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            RemainingTimer.SetTimer(TimeSpan.FromMinutes(45));

        }

        private ApplicationViewModel ApplicationViewModel { get; set; }

        public void ChangePageView(BaseViewModel viewModel)
        {
            ApplicationViewModel.ChangePageViewModel(viewModel);

            //if (viewModel.GetType() == typeof(CarSelectionViewModel))
            //{
            //    ApplicationViewModel.TitleLabel = "Car Selection";
            //    TitleLabelTextBlock.Margin = new Thickness(10, 10, 10, 10);
            //    BackButton.Visibility = Visibility.Hidden;
            //    return;
            //}

            //if (viewModel.GetType() == typeof(LearningViewModel))
            //    ApplicationViewModel.TitleLabel = "Learning Mode";
            //if (viewModel.GetType() == typeof(TestingViewModel))
            //    ApplicationViewModel.TitleLabel = "Testing Mode";
            //if (viewModel.GetType() == typeof(SimulatorViewModel))
            //    ApplicationViewModel.TitleLabel = "Simulator";

            //BackButton.Visibility = Visibility.Visible;
            //TitleLabelTextBlock.Margin = new Thickness(45, 10, 10, 10);
        }

        private void RefreshOptoConnection(bool isConnected)
        {
            if (ApplicationViewModel.CurrentPageViewModel == null) return;

            //if (ApplicationViewModel.CurrentPageViewModel.GetType() != typeof(CarSelectionViewModel)) return;

            //var viewModel = (CarSelectionViewModel)ApplicationViewModel.CurrentPageViewModel;
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

            //if (System.Windows.Application.Current.Windows.OfType<OptoMonitorView>().Any())
            //    OptoMonitorView.Close();

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
            //AddManufacturerFlyout.FindChild<NewManufacturerFlyout>("NewManufacturerFlyout").NewManufacturerFlyoutViewModel.ApplicationSessionFactory =
            //    ViewModel.ApplicationSessionFactory;
            AlarmConfiguration.Tag = ControlConstants.ChangeTracking.Pristine;
            AlarmConfiguration.IsOpen = true;
            //ConfigurationSettingsFlyout.ClosingFinished += AddManufacturerFlyoutOnClosingFinished;
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

        private void StartPhaseButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartTest.ToInt(), 1);
        }
    }
}
