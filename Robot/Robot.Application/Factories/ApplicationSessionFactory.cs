using Robot.Application.Services;
using Robot.Application.Session;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Factories
{
    public interface IApplicationSessionFactory
    {
        ControlWindowViewModel ControlWindowViewModel { get; }
        IOptoMmpFactory OptoMmpFactory { get; }
        ILoggingService LoggingService { get; }
        IScratchPadFactory ScratchPadFactory { get; }
        //bool IsConnectedToOpto { get; set; }
        StatusConstants.ConnectionStatus OptoConnectionStatus { get; set; }
    }

    public class ApplicationSessionFactory : IApplicationSessionFactory
    {
        public ControlWindowViewModel ControlWindowViewModel { get { return ApplicationSession.ControlWindowViewModel; } }

        public IOptoMmpFactory OptoMmpFactory { get { return ApplicationSession.OptoMmpFactory; } }

        public ILoggingService LoggingService { get { return ApplicationSession.LoggingService; } }

        public IScratchPadFactory ScratchPadFactory { get { return ApplicationSession.ScratchPadFactory; } }

        public bool IsConnectedToOpto { get { return ApplicationSession.IsConnectedToOpto; } set { ApplicationSession.IsConnectedToOpto = value; }}

        public StatusConstants.ConnectionStatus OptoConnectionStatus { 
            get { return ApplicationSession.ControlWindowViewModel.OptoConnectionStatus; }
            set { ApplicationSession.ControlWindowViewModel.OptoConnectionStatus = value; } 
        }
    }
}
