using System.Configuration;
using System.Threading.Tasks;
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
            ApplicationViewModel = new ApplicationViewModel() {AddManufacturerFlyout = AddManufacturerFlyout};
            ApplicationSessionFactory = new ApplicationSessionFactory() { ApplicationViewModel = ApplicationViewModel };
            
            DataContext = ApplicationViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            // ToggleOptoConnection();
            ChangePageView(new CarSelectionViewModel(ApplicationSessionFactory));
        }

        public void ChangePageView(BaseViewModel viewModel)
        {
            ApplicationViewModel.ChangePageViewModel(viewModel);
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
    }
}
