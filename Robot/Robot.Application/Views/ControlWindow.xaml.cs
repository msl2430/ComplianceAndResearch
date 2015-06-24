using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using Robot.Application.Factories;
using Robot.Application.Services;
using Robot.Application.Session;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Views
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow
    {
        public override void BeginInit()
        {
            base.BeginInit();
            InitializeComponents();           
        }

        private void InitializeComponents()
        {
            ApplicationSessionFactory = new ApplicationSessionFactory();
            ApplicationSessionFactory.ControlWindowViewModel.StatusLabel = "Disconnected";
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            DataContext = ApplicationSessionFactory.ControlWindowViewModel;
            ToggleOptoConnection();
            
        }

        private void ToggleOptoConnection()
        {
            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Disconnected:
                    Task.Run(() =>
                    {
                        ApplicationSessionFactory.LoggingService.LogEvent("Connecting to Opto 22 @" + ConfigurationManager.AppSettings["OptoIpAddress"], true);
                        OptoConnectionWorker.DoWork();
                    }).ConfigureAwait(false);
                    break;
                case StatusConstants.ConnectionStatus.Connected:
                    ApplicationSessionFactory.LoggingService.LogEvent("Disconnecting from Opto 22", true);
                    OptoConnectionWorker.CancelWork();
                    break;
            }
        }

        #region Events
        private void OptoConnectionToggle_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            ToggleOptoConnection();
        }
        #endregion
    }
}
