using System;
using System.IO;
using System.Reflection;
using Robot.Application.ViewModels;

namespace Robot.Application.Services
{
    public static class LoggingService 
    {
        private static ControlWindowViewModel ControlWindowModel { get; set; }
        private static readonly string _logDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Logs";

        private static string LogFilePath
        {
            get { return _logDirectory + string.Format("\\Log_{0}.txt", DateTime.Now.ToString("MM_dd_yyyy")); }
        }

        public static void SetControlWindoModel(ControlWindowViewModel model)
        {
            ControlWindowModel = model;
        }

        public static void LogEvent(string message, bool includeInAppWindow = false)
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);

            using (var file = new StreamWriter(LogFilePath, true))
            {
                file.WriteLine("{0} >> {1}", DateTime.Now.ToLongTimeString(), message);
            }
            if(includeInAppWindow && ControlWindowModel != null)
                ControlWindowModel.LogWindowString = message;
        }
    }
}
