using System;
using System.Windows;
using Opto22.Core.Service;
using Robot.Application.Factories;
using Robot.Application.Services;
using Robot.Application.Services.WorkerServices;

namespace Robot.Application
{
    public class BaseWindow : Window
    {
        private IOptoMmpFactory _optoMmpFactory { get; set; }
        public IOptoMmpFactory OptoMmpFactory { get { return _optoMmpFactory ?? (_optoMmpFactory = new OptoMmpFactory()); }}

        private IPointService _pointService { get; set; }
        public IPointService PointService { get { return _pointService ?? (_pointService = new PointService(OptoMmpFactory.Current)); } }

        private IScratchPadService _scratpaService { get; set; }
        public IScratchPadService ScratchPadService { get { return _scratpaService ?? (_scratpaService = new ScratchPadService(OptoMmpFactory.Current)); } }

        public OptoConnectionWorkerService OptoConnectionWorker { get; set; }
        public InitializeDataWorkerService InitializeDataWorkerService { get; set; }
    }
}
