using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Robot.Application.Factories;
using Robot.Application.Services.WorkerServices;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Views
{
    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : BaseWindowView
    {
        private ApplicationViewModel ApplicationViewModel { get; set; }

        public ApplicationView()
        {
            InitializeComponent();
            ApplicationViewModel = new ApplicationViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory() { ApplicationViewModel = ApplicationViewModel };
            
            DataContext = ApplicationViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            // ToggleOptoConnection();
            ChangePageView(new CarSelectionViewModel(ApplicationSessionFactory));
        }

        public void ChangePageView(BaseViewModel viewModel)
        {
            ApplicationViewModel.ChangePageViewModel(viewModel);
            if (viewModel.GetType() == typeof (CarSelectionViewModel))
            {
                ApplicationViewModel.TitleLabel = "Car Selection";
                TitleLabelTextBlock.Margin = new Thickness(10,10,10,10);
                BackButton.Visibility = Visibility.Hidden;
                return;
            }
            if (viewModel.GetType() == typeof (LearningViewModel))
                ApplicationViewModel.TitleLabel = "Learning Mode";

            BackButton.Visibility = Visibility.Visible;
            TitleLabelTextBlock.Margin = new Thickness(45, 10, 10, 10);
        }
        
        private void ToggleOptoConnection()
        {
            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Disconnected:
                    Task.Run(() =>
                    {
                        ApplicationSessionFactory.LogEvent("Connecting to Opto 22 @" + ConfigurationManager.AppSettings["OptoIpAddress"], true);
                        OptoConnectionWorker.DoWork();
                    }).ConfigureAwait(false);
                    break;
                case StatusConstants.ConnectionStatus.Connected:
                    ApplicationSessionFactory.LogEvent("Disconnecting from Opto 22", true);
                    OptoConnectionWorker.CancelWork();
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

            if (lastType == typeof (CarSelectionViewModel)) 
                ChangePageView(new CarSelectionViewModel(ApplicationSessionFactory));
        }
    }
}
