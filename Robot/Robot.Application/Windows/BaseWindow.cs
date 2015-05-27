﻿using System;
using System.Windows;
using Opto22.Core.Service;
using Robot.Application.Factories;
using Robot.Application.Services;

namespace Robot.Application
{
    public class BaseWindow : Window
    {
        private IOptoMmpFactory _optoMmpFactory { get; set; }
        public IOptoMmpFactory OptoMmpFactory { get { return _optoMmpFactory ?? (_optoMmpFactory = new OptoMmpFactory()); }}

        private IPointService _pointService { get; set; }
        public IPointService PointService { get { return _pointService ?? (_pointService = new PointService(OptoMmpFactory.Current)); } }

        public OptoConnectionWorkerService OptoConnectionWorker { get; set; }
        public InitializeDataWorkerService InitializeDataWorkerService { get; set; }
    }
}