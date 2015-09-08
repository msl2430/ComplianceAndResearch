using MahApps.Metro.Controls;
using Robot.Application.Factories;
using Robot.Application.Services.DataServices;
using Robot.Application.Services.WorkerServices;

namespace Robot.Application.Views
{
    public class BaseWindowView : MetroWindow
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; } 

        public IOptoConnectionWorkerService OptoConnectionWorker { get; set; }

        private IManufacturerService _manufacturerService { get; set; }
        public IManufacturerService ManufacturerService => _manufacturerService ?? (_manufacturerService = new ManufacturerService());

        private IModelService _modelService { get; set; }
        public IModelService ModelService => _modelService ?? (_modelService = new ModelService());

        public InitializeDataWorkerService InitializeDataWorkerService { get; set; }
    }
}
