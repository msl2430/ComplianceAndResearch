using EngineCell.Application.ViewModels;
using EngineCell.Core.Constants;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;

namespace EngineCell.Application.Views
{
    public partial class ApplicationView : BaseWindowView
    {
        private ApplicationViewModel ApplicationViewModel { get; set; }

        private int _numberOfTests = 1;

        public ApplicationView()
        {
            InitializeComponent();
            ApplicationViewModel = new ApplicationViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory() { ApplicationViewModel = ApplicationViewModel };

            DataContext = ApplicationViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            
            //ToggleOptoConnection();
            //ChangePageView(new CarSelectionViewModel(ApplicationSessionFactory) { IsOptoConnected = false });
            //ChangePageView(new SimulatorViewModel(ApplicationSessionFactory));

            //OptoMonitorView = new OptoMonitorView(ApplicationSessionFactory);
            //OptoMonitorView.Show();
        }

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

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var lastType = ApplicationViewModel.PageViewModelNavigationPath.Last().GetType();
            ApplicationViewModel.PageViewModelNavigationPath.RemoveAt(ApplicationViewModel.PageViewModelNavigationPath.Count - 1);

            //if (lastType == typeof(CarSelectionViewModel))
            //    ChangePageView(new CarSelectionViewModel(ApplicationSessionFactory) { IsOptoConnected = ApplicationViewModel.IsConnected });
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

        private void CreateNewTest(int testIndex)
        {
            var grpBox = new GroupBox() { Header = "Test " + (testIndex+1), Visibility = Visibility.Visible, Width = 175, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(5 + (185 * testIndex), 0, 0, 5) };
            var grid = new Grid();
            var stackPanel = new StackPanel();
            var coolantSetPoinTextbox = new TextBox() { Width = 75 };
            var dynoSetPointTextbox = new TextBox() {Width = 75};
            var throttleSetPointTextbox = new TextBox() { Width = 75 };
            var testRunTimeTextbox = new TextBox() {Width = 75};
            stackPanel.Children.Add(new Label() {Content = "Coolant Set Point"});
            stackPanel.Children.Add(coolantSetPoinTextbox);
            stackPanel.Children.Add(new Label() { Content = "Dyno Set Point" });
            stackPanel.Children.Add(dynoSetPointTextbox);
            stackPanel.Children.Add(new Label() {Content = "Throttle Set Point Label"});
            stackPanel.Children.Add(throttleSetPointTextbox);
            stackPanel.Children.Add(new Label() {Content = "Test Run Time (minutes)"});
            stackPanel.Children.Add(testRunTimeTextbox);
            grid.Children.Add(stackPanel);
            grpBox.Content = grid;
            TestDisplay.Children.Add(grpBox);

        }

        private void AddTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateNewTest(_numberOfTests);
            _numberOfTests++;
        }
    }
}
