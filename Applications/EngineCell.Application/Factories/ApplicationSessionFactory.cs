using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.Services;
using EngineCell.Application.Session;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.Factories
{
    public interface IApplicationSessionFactory
    {
        IOptoMmpFactory OptoMmpFactory { get; }
        IScratchPadFactory ScratchPadFactory { get; }
        StatusConstants.ConnectionStatus OptoConnectionStatus { get; set; }

        CellTestModel CurrentCellTest { get; set; }
        IList<PointDataModel> CellPoints { get; set; }

        MainWindowViewModel ApplicationViewModel { get; set; }

        bool IsLogWindowVisible { get; set; }
        void LogEvent(string message, bool includeInAppWindow = false);
    }

    public class ApplicationSessionFactory : IApplicationSessionFactory
    {
        public IOptoMmpFactory OptoMmpFactory { get { return ApplicationSession.OptoMmpFactory; } }
        public IScratchPadFactory ScratchPadFactory { get { return ApplicationSession.ScratchPadFactory; } }
        public StatusConstants.ConnectionStatus OptoConnectionStatus
        {
            get { return ApplicationSession.ApplicationViewModel.OptoConnectionStatus; }
            set { ApplicationSession.ApplicationViewModel.OptoConnectionStatus = value; }
        }

        public CellTestModel CurrentCellTest { get; set; }
        public IList<PointDataModel> CellPoints { get; set; }

        public MainWindowViewModel ApplicationViewModel
        {
            get { return ApplicationSession.ApplicationViewModel; }
            set
            {
                LoggingService.SetApplicationViewModel(value);
                ApplicationSession.ApplicationViewModel = value;
            }
        }

        public bool IsLogWindowVisible { get; set; }
        public void LogEvent(string message, bool includeInAppWindow = false)
        {
            if (!IsLogWindowVisible)
                Task.Run(() => LogToStatusWindow(message));

            LoggingService.LogEvent(message, includeInAppWindow);
        }

        private void LogToStatusWindow(string message)
        {
            var tempStatusLabel = ApplicationViewModel.StatusLabel;
            ApplicationViewModel.StatusLabel = message;
            Thread.Sleep(2500);
            ApplicationViewModel.StatusLabel = tempStatusLabel;
        }
    }
}
