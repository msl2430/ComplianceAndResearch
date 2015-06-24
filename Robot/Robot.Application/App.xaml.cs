using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Robot.Application.Factories;
using Robot.Application.Session;

namespace Robot.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ApplicationSession.LoggingService.LogEvent("Application started");
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            ApplicationSession.LoggingService.LogEvent("Application shut down");
        }
    }
}
