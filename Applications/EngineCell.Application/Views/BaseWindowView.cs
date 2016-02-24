using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using MahApps.Metro.Controls;

namespace EngineCell.Application.Views
{
    public class BaseWindowView : MetroWindow
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        public IOptoConnectionWorkerService OptoConnectionWorker { get; set; }

        public IPointWorkerService PointWorkerService { get; set; }
    }
}
