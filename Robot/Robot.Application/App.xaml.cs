using System.Windows;
using Robot.Application.Services;

namespace Robot.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            LoggingService.LogEvent("Application started");
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            LoggingService.LogEvent("Application shut down");
        }
    }
}
