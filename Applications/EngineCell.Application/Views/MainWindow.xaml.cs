using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Welcome;
using EngineCell.Application.Views.Welcome;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
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

        private ICellPointRepository _cellPointRepository { get; set; }
        private ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = new MainWindowViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory()
            {
                ApplicationViewModel = MainWindowViewModel,
                Cells = CellPointRepository.GetCells()
            };

            if (ApplicationSessionFactory.Cells.IsNullOrEmpty())
            {
                ApplicationSessionFactory.LogEvent("CRITICAL ERROR: No cells found in database.", true);
                return;
            }

            CellComboBox.ItemsSource = ApplicationSessionFactory.Cells;

            MainWindowViewModel.PointConfigViewModel = new PointConfigurationViewModel(ApplicationSessionFactory);
            MainWindowViewModel.TestDisplayViewModel = new TestDisplayViewModel(ApplicationSessionFactory, new StripChartViewModel(ApplicationSessionFactory));
            MainWindowViewModel.TestDisplayViewModel.PointWorkerService = new PointWorkerService(MainWindowViewModel.TestDisplayViewModel, Dispatcher);
            MainWindowViewModel.PhaseConfigViewModel = new PhaseConfigViewModel(ApplicationSessionFactory);
            MainWindowViewModel.WelcomeViewModel = new WelcomeScreenViewModel(ApplicationSessionFactory, this);
            MainWindowViewModel.ViewModels = new ObservableCollection<BaseViewModel>() { MainWindowViewModel.PointConfigViewModel, MainWindowViewModel.TestDisplayViewModel, MainWindowViewModel.PhaseConfigViewModel, MainWindowViewModel.WelcomeViewModel };

            if (Properties.Settings.Default.LastCellId > 0)
            {
                var cell = ApplicationSessionFactory.Cells.FirstOrDefault(c => c.CellId == Properties.Settings.Default.LastCellId);
                if (cell != null)
                {
                    CellComboBox.SelectedItem = cell;
                    SelectCell(cell);
                }
            }

            if (ApplicationSessionFactory.CurrentCell == null)
            {
                OptoConnectionToggle.IsEnabled = false;
                PointConfig.IsEnabled = false;
            }

            //TESTING
            var test = CellPointRepository.GetCellTestById(2007);
            ApplicationSessionFactory.CurrentCellTest = test;

            if (ApplicationSessionFactory.CurrentCell == null || ApplicationSessionFactory.CurrentCellTest == null)
            {
                ChangePageView(MainWindowViewModel.WelcomeViewModel);
            }
            else
            {
                UpdateViewModels();
                ChangePageView(MainWindowViewModel.TestDisplayViewModel);
            }
            
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
            MainWindowViewModel.HasTestActive = ApplicationSessionFactory.CurrentCellTest != null;

            UpdateTestName(MainWindowViewModel.HasTestActive ? ApplicationSessionFactory.CurrentCellTest.Name : "");

            MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == viewModel.GetType()).ZIndex = 1;
        }

        public void ChangePageView(ControlConstants.Views view)
        {
            foreach (var model in MainWindowViewModel.ViewModels)
            {
                model.ZIndex = 0;
            }

            MainWindowViewModel.HasTestActive = ApplicationSessionFactory.CurrentCellTest != null;

            UpdateTestName(MainWindowViewModel.HasTestActive ? ApplicationSessionFactory.CurrentCellTest.Name : "");

            switch (view)
            {
                case ControlConstants.Views.Welcome:
                    MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == typeof(WelcomeScreenViewModel)).ZIndex = 1;
                    break;
                case ControlConstants.Views.TestDisplay:
                    MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == typeof(TestDisplayViewModel)).ZIndex = 1;
                    break;
                case ControlConstants.Views.PointConfiguration:
                    MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == typeof(PointConfigurationViewModel)).ZIndex = 1;
                    break;
                case ControlConstants.Views.PhaseConfiguration:
                    MainWindowViewModel.ViewModels.FirstOrDefault(vm => vm.GetType() == typeof(PhaseConfigViewModel)).ZIndex = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(view), view, null);
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
            UpdateViewModels();
            ChangePageView(MainWindowViewModel.TestDisplayViewModel);
        }
        
        private void MenuPhaseConfig_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateViewModels();
            ChangePageView(MainWindowViewModel.PhaseConfigViewModel);
        }

        private void OptoConnectionToggle_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            ToggleOptoConnection();
        }

        private void ApplicationView_OnClosing(object sender, CancelEventArgs e)
        {
            if (ApplicationSessionFactory.OptoConnectionStatus == StatusConstants.ConnectionStatus.Connected)
                ToggleOptoConnection();

            while (ApplicationSessionFactory.OptoConnectionStatus == StatusConstants.ConnectionStatus.Connected)
            {
                Thread.Sleep(250);
            }
        }

        private void CellComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectCell((CellModel)CellComboBox.SelectedItem);
        }

        private void MenuNewTest_Clieck(object sender, RoutedEventArgs e)
        {
            var dialog = new NewTestDialog();
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.NewTestName))
                return;

            var test = CellPointRepository.CreateCellTest(ApplicationSessionFactory.CurrentCell.CellId, dialog.NewTestName, dialog.NewTestDescription);
            ApplicationSessionFactory.CurrentCellTest = test;
            UpdateViewModels();
            ChangePageView(ControlConstants.Views.TestDisplay);
        }

        private void MenuCloseTest_Clieck(object sender, RoutedEventArgs e)
        {
            ApplicationSessionFactory.CurrentCellTest = null;
            UpdateTestName("");
            UpdateViewModels();
            ChangePageView(ControlConstants.Views.Welcome);
        }

        private void MenuViewHistory_Clieck(object sender, RoutedEventArgs e)
        {
            var dialog = new TestHistoryDialog(ApplicationSessionFactory);
            dialog.ShowDialog();

            if (dialog.ViewModel.TestToCopy == null)
                return;

            var test = CellPointRepository.CreateCellTest(ApplicationSessionFactory.CurrentCell.CellId, dialog.ViewModel.TestToCopy.Name + " - Copy", dialog.ViewModel.TestToCopy.Description);
            foreach (var phase in dialog.ViewModel.TestToCopy.Phases)
            {
                var savedPhase = WidgetRepository.AddPhaseToTest(test.CellTestId, phase.PhaseOrder, phase.Name);
                foreach (var widget in phase.Widgets)
                {
                    var savedWidget = WidgetRepository.AddWidgetToPhase(savedPhase.CellTestPhaseId, widget.WidgetId);
                    foreach (var setting in widget.Settings)
                    {
                        var widgetSetting = WidgetRepository.SaveWidgetSetting(savedWidget.CellTestPhaseWidgetId, setting.WidgetSettingId, setting.Value);
                        savedWidget.Settings.Add(widgetSetting);
                    }
                    foreach (var setting in widget.PhaseEndSettings)
                    {
                        var endSetting = WidgetRepository.SavePhaseEndSetting(savedWidget.CellTestPhaseWidgetId, setting.PhaseEndSettingId, setting.Value);
                        savedWidget.PhaseEndSettings.Add(endSetting);
                    }
                    savedPhase.Widgets.Add(savedWidget);
                }
                test.Phases.Add(savedPhase);
            }
            ApplicationSessionFactory.CurrentCellTest = test;
            UpdateViewModels();
            ChangePageView(ControlConstants.Views.TestDisplay);
        }
        #endregion

        #region Private Methods
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

        private void UpdateTestName(string name)
        {
            MainWindowViewModel.CellTestName = name;
            TestName.Text = MainWindowViewModel.CellTestName.Left(30, true, true);
        }

        private void RefreshOptoConnection(bool isConnected)
        {
            //if (MainWindowViewModel.CurrentPageViewModel == null) return;

            //if (MainWindowViewModel.CurrentPageViewModel.GetType() != typeof(CarSelectionViewModel)) return;

            //var viewModel = (CarSelectionViewModel)MainWindowViewModel.CurrentPageViewModel;
            //viewModel.IsOptoConnected = isConnected;
        }

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

        private void SelectCell(CellModel cell)
        {
            IsEnabled = false;
            ApplicationSessionFactory.CurrentCell = cell;
            var points = CellPointRepository.GetCellPointsByCellId(ApplicationSessionFactory.CurrentCell.CellId);

            if (points.IsNullOrEmpty())
            {
                ApplicationSessionFactory.LogEvent("CRITICAL ERROR: No points associated with selected cell.");
                return;
            }

            Properties.Settings.Default.LastCellId = cell.CellId;
            Properties.Settings.Default.Save();

            ApplicationSessionFactory.CellPoints = points
                .ToList()
                .Select(p => new PointDataModel(p))
                .ToList();

            MainWindowViewModel.PointConfigViewModel.UpdateViewModel();
            MainWindowViewModel.WelcomeViewModel.UpdateViewModel();
            UpdateViewModels();
            OptoConnectionToggle.IsEnabled = true;
            PointConfig.IsEnabled = true;
            IsEnabled = true;
        }

        public void UpdateViewModels()
        {
            MainWindowViewModel.TestDisplayViewModel.UpdateViewModel();
            MainWindowViewModel.PhaseConfigViewModel.UpdateViewModel();            
        }
        #endregion

        private void EditTestName_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new NewTestDialog(ApplicationSessionFactory.CurrentCellTest.Name, ApplicationSessionFactory.CurrentCellTest.Description) { Title = "Edit Test Name"};
            dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.NewTestName) || (dialog.NewTestName == ApplicationSessionFactory.CurrentCellTest.Name && dialog.NewTestDescription == ApplicationSessionFactory.CurrentCellTest.Description))
                return;
            ApplicationSessionFactory.CurrentCellTest.Name = dialog.NewTestName;
            ApplicationSessionFactory.CurrentCellTest.Description = dialog.NewTestDescription;
            UpdateTestName(ApplicationSessionFactory.CurrentCellTest.Name);
            CellPointRepository.UpdateCellTestName(ApplicationSessionFactory.CurrentCellTest.CellTestId, ApplicationSessionFactory.CurrentCellTest.Name, ApplicationSessionFactory.CurrentCellTest.Description);
        }
    }
}
