
using System.Collections.Generic;
using Robot.Application.Factories;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Models.Models;

namespace Robot.Application.ViewModels
{
    public class LearningViewModel : BaseViewModel
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string CarDisplayName { get { return string.Format("{0} {1} {2}", ModelYear, ManufacturerName, ModelName); } }
        public IList<ThrottleSetPointChartModel> SetPointCharts { get; set; }
        public RoadTestChartModel LearningPhaseRoadTest { get; set; }

        public int _learningProgressStatus { get; set; }

        public int LearningProgressStatus
        {
            get { return _learningProgressStatus; }
            set
            {
                _learningProgressStatus = value;
                switch (_learningProgressStatus)
                {
                    case (int) StatusConstants.PhaseStatus.PreCheckActive:
                        PreCheckStatus = StatusConstants.ProgressStatus.Active.ToInt();
                        break;
                    case (int) StatusConstants.PhaseStatus.LoadDataPointActive:
                        PreCheckStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        LoadingDataStatus = StatusConstants.ProgressStatus.Active.ToInt();
                        break;
                    case (int) StatusConstants.PhaseStatus.Running:
                        PreCheckStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        LoadingDataStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        RunningStatus = StatusConstants.ProgressStatus.Active.ToInt();
                        break;
                    case (int) StatusConstants.PhaseStatus.Shutdown:
                        PreCheckStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        LoadingDataStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        RunningStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        ShutdownStatus = StatusConstants.ProgressStatus.Active.ToInt();
                        break;
                    case (int) StatusConstants.PhaseStatus.Completed:
                        PreCheckStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        LoadingDataStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        RunningStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        ShutdownStatus = StatusConstants.ProgressStatus.Finished.ToInt();
                        break;
                    default:
                        PreCheckStatus = StatusConstants.ProgressStatus.InActive.ToInt();
                        LoadingDataStatus = StatusConstants.ProgressStatus.InActive.ToInt();
                        RunningStatus = StatusConstants.ProgressStatus.InActive.ToInt();
                        ShutdownStatus = StatusConstants.ProgressStatus.InActive.ToInt();
                        break;
                }
            }
        }


        public LearningViewModel(IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
        }

        #region Progress Status
        public int _preCheckStatus { get; set; }
        public int PreCheckStatus { get { return _preCheckStatus; } set { _preCheckStatus = value; OnPropertyChanged("PreCheckStatus"); } }

        public int _loadingDataStatus { get; set; }
        public int LoadingDataStatus { get { return _loadingDataStatus; } set { _loadingDataStatus = value; OnPropertyChanged("LoadingDataStatus"); } }

        public int _runningStatus { get; set; }
        public int RunningStatus { get { return _runningStatus; } set { _runningStatus = value; OnPropertyChanged("RunningStatus"); } }

        public int _shutdownStatus { get; set; }
        public int ShutdownStatus { get { return _shutdownStatus; } set { _shutdownStatus = value; OnPropertyChanged("ShutdownStatus"); } }
        #endregion
    }
}
