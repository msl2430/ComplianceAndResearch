using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Opto22.Core.Constants;
using Robot.Application.Services.DataServices;
using Robot.Application.Services.WorkerServices;
using Robot.Application.ViewModels;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Models.Helpers;
using Robot.Models.Models;

namespace Robot.Application.Views.Learning
{
    /// <summary>
    /// Interaction logic for LearningView.xaml
    /// </summary>
    public partial class LearningView : BaseUserControl
    {
        private LearningViewModel ViewModel { get; set; }
        private ILearningWorkerService LearningWorkerService { get; set; }

        public LearningView()
        {
            InitializeComponent();
        }

        private void LearningView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(DataContext != null)
                ViewModel = (LearningViewModel)DataContext;

            ViewModel.LearningPhaseRoadTest = RoadTestService.GetLearningRoadTests()[0]; //TODO user should select this before coming here;

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.GoToLearningPhase.ToInt(), 1);
            ViewModel.LearningProgressStatus  = StatusConstants.PhaseStatus.InActive.ToInt();
        }

        private void StartLearningPhaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartLearningPhaseButton.IsEnabled = false;
            Task.Run(() =>
            {
                var tspModel = SetPointService.GetSetPointChartByModelId(1); //TODO Need to have user select a model
                if (tspModel == null)
                    throw new Exception("Error getting set point chart.");
                ViewModel.ApplicationSessionFactory.LogEvent("Starting Learn Phase", true);
                LearningWorkerService = new LearningWorkerService(ViewModel, Dispatcher) { TspModel = tspModel};
                LearningWorkerService.DoWork();
            }).ContinueWith((x) =>
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Learning Phase completed", true);
                ViewModel.LearningProgressStatus = StatusConstants.PhaseStatus.InActive.ToInt();
                Dispatcher.Invoke(() => StartLearningPhaseButton.IsEnabled = true);
            }).ConfigureAwait(false);
        }
    }
}
