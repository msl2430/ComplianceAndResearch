using Robot.Application.Services;
using Robot.Application.Session;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Factories
{
    public interface IApplicationSessionFactory
    {
        IOptoMmpFactory OptoMmpFactory { get; }
        IScratchPadFactory ScratchPadFactory { get; }
        StatusConstants.ConnectionStatus OptoConnectionStatus { get; set; }
        ControlWindowViewModel ControlWindowViewModel { get; set; }
        void LogEvent(string message, bool includeInAppWindow = false);
    }

    public class ApplicationSessionFactory : IApplicationSessionFactory
    {
        public IOptoMmpFactory OptoMmpFactory { get { return ApplicationSession.OptoMmpFactory; } }

        public IScratchPadFactory ScratchPadFactory { get { return ApplicationSession.ScratchPadFactory; } }

        public StatusConstants.ConnectionStatus OptoConnectionStatus { 
            get { return ApplicationSession.ControlWindowViewModel.OptoConnectionStatus; }
            set { ApplicationSession.ControlWindowViewModel.OptoConnectionStatus = value; } 
        }

        public ControlWindowViewModel ControlWindowViewModel
        {
            get { return ApplicationSession.ControlWindowViewModel; }
            set
            {
                LoggingService.SetControlWindoModel(value);
                ApplicationSession.ControlWindowViewModel = value;
            }
        }
        
        public void LogEvent(string message, bool includeInAppWindow = false)
        {
            LoggingService.LogEvent(message, includeInAppWindow);
        }
    }
}
