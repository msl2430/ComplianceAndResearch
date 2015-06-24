using System;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using Robot.Application.ViewModels;

namespace Robot.Application.Services
{
    public interface ILoggingService
    {
        void LogEvent(string message, bool includeInAppWindow = false);
    }

    public sealed class LoggingService : ILoggingService
    {
        private ControlWindowViewModel ControlWindowModel { get; set; }
        private readonly string _logDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Logs";

        private string LogFilePath
        {
            get { return _logDirectory + string.Format("\\Log_{0}.txt", DateTime.Now.ToString("MM_dd_yyyy")); }
        }

        public LoggingService(ControlWindowViewModel model)
        {
            ControlWindowModel = model;
        }

        public void LogEvent(string message, bool includeInAppWindow = false)
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);

            using (var file = new StreamWriter(LogFilePath, true))
            {
                file.WriteLine("{0} >> {1}", DateTime.Now.ToLongTimeString(), message);
            }
            if(includeInAppWindow)
                ControlWindowModel.LogWindowString = message;
        }
    }
}
