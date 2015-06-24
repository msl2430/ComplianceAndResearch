using MahApps.Metro.Controls;
using Robot.Application.Factories;
using Robot.Application.Services;

namespace Robot.Application.Views
{
    public class BaseWindowView : MetroWindow
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; }
        public IOptoConnectionWorkerService OptoConnectionWorker { get; set; }

        public InitializeDataWorkerService InitializeDataWorkerService { get; set; }
    }
}
