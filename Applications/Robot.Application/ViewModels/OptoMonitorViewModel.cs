
using Opto22.Core.Constants;

namespace Robot.Application.ViewModels
{
    public class OptoMonitorViewModel : BaseViewModel 
    {
        private bool _isApplicationConnected { get; set; }
        public bool IsApplicationConnected
        {
            get { return _isApplicationConnected; }
            set { _isApplicationConnected = value; OnPropertyChanged("IsApplicationConnected"); }
        }

        private bool _isGoToLearningPhase { get; set; }
        public bool IsGoToLearningPhase
        {
            get { return _isGoToLearningPhase; }
            set { _isGoToLearningPhase = value; OnPropertyChanged("IsGoToLearningPhase"); }
        }

        private bool _isGoToTestPhase { get; set; }
        public bool IsGoToTestPhase
        {
            get { return _isGoToTestPhase; }
            set { _isGoToTestPhase = value; OnPropertyChanged("IsGoToTestPhase"); }
        }

        private int _templateLoadStatus { get; set; }
        public int TemplateLoadStatus
        {
            get { return _templateLoadStatus; }
            set
            {
                var original = _templateLoadStatusString;
                _templateLoadStatus = value; 
                SetTemplateStatusString();
                if(original != _templateLoadStatusString)
                    ApplicationSessionFactory.LogEvent(string.Format("Template load status changed from '{0}' to '{1}'", original, _templateLoadStatusString));
                OnPropertyChanged("TemplateLoadStatus");
            }
        }

        private string _templateLoadStatusString { get; set; }
        public string TemplateLoadStatusString
        {
            get { return _templateLoadStatusString; }
            set { _templateLoadStatusString = value; OnPropertyChanged("TemplateLoadStatusString"); }
        }

        private int _selectedManufacturerId { get; set; }
        public int SelectedManufacturerId
        {
            get { return _selectedManufacturerId; }
            set { _selectedManufacturerId = value; OnPropertyChanged("SelectedManufacturerId"); }
        }

        private int _selectedModelId { get; set; }
        public int SelectedModelId
        {
            get { return _selectedModelId; }
            set { _selectedModelId = value; OnPropertyChanged("SelectedModelId"); }
        }

        private int _selectedModelYear { get; set; }
        public int SelectedModelYear
        {
            get { return _selectedModelYear; }
            set { _selectedModelYear = value; OnPropertyChanged("SelectedModelYear"); }
        }

        #region Test Phase Data
        private decimal _testPhaseCurrentRunTime { get; set; }
        public decimal TestPhaseCurrentRunTime
        {
            get { return _testPhaseCurrentRunTime; }
            set { _testPhaseCurrentRunTime = value; OnPropertyChanged("TestPhaseCurrentRunTime"); }
        }

        private decimal _testPhaseCurrentSpeed { get; set; }
        public decimal TestPhaseCurrentSpeed
        {
            get { return _testPhaseCurrentSpeed; }
            set { _testPhaseCurrentSpeed = value; OnPropertyChanged("TestPhaseCurrentSpeed"); }
        }

        private decimal _testPhaseCurrentAcceleration { get; set; }
        public decimal TestPhaseCurrentAcceleration
        {
            get { return _testPhaseCurrentAcceleration; }
            set { _testPhaseCurrentAcceleration = value; OnPropertyChanged("TestPhaseCurrentAcceleration"); }
        }

        private decimal _testPhaseRequiredSpeed { get; set; }
        public decimal TestPhaseRequiredSpeed
        {
            get { return _testPhaseRequiredSpeed; }
            set { _testPhaseRequiredSpeed = value; OnPropertyChanged("TestPhaseRequiredSpeed"); }
        }

        private decimal _testPhaseThrottleSetPoint { get; set; }
        public decimal TestPhaseThrottleSetPoint
        {
            get { return _testPhaseThrottleSetPoint; }
            set { _testPhaseThrottleSetPoint = value; OnPropertyChanged("TestPhaseThrottleSetPoint"); }
        }
        #endregion

        private int _strategyLocation { get; set; }
        public int StrategyLocation
        {
            get { return _strategyLocation; }
            set
            {
                var original = _strategyLocationString;
                _strategyLocation = value; 
                SetStrategyLocationString();
                if (original != _strategyLocationString)
                    ApplicationSessionFactory.LogEvent(string.Format("Strategy location changed from '{0}' to '{1}'.", original, _strategyLocationString), true);
                OnPropertyChanged("StrategyLocation");
            }
        }

        private string _strategyLocationString { get; set; }
        public string StrategyLocationString
        {
            get { return _strategyLocationString; }
            set { _strategyLocationString = value; OnPropertyChanged("StrategyLocationString"); }
        }

        private void SetStrategyLocationString()
        {
            switch (_strategyLocation)
            {
                case (int)ScratchPadConstants.StrategyLocationValues.AppRunningCheck:
                    StrategyLocationString = "Waiting for application to connect";
                    break;
                case (int)ScratchPadConstants.StrategyLocationValues.PhaseCheck:
                    StrategyLocationString = "Waiting for phase selection";
                    break;
                case (int)ScratchPadConstants.StrategyLocationValues.LearningPhase:
                    StrategyLocationString = "Learning phase started";
                    break;
                case (int)ScratchPadConstants.StrategyLocationValues.LearningPhaseCalculating:
                    StrategyLocationString = "Calculating toggle set point";
                    break;
                case (int)ScratchPadConstants.StrategyLocationValues.TestPhase:
                    StrategyLocationString = "Test phase started";
                    break;
                case (int)ScratchPadConstants.StrategyLocationValues.Simulator:
                    StrategyLocationString = "Simulator active";
                    break;
                default:
                    StrategyLocationString = "Off";
                    break;
            }
        }

        private void SetTemplateStatusString()
        {
            switch (_templateLoadStatus)
            {
                case (int)ScratchPadConstants.LoadStatus.Waiting:
                    TemplateLoadStatusString = "Waiting for template";
                    break;
                case (int)ScratchPadConstants.LoadStatus.Loading:
                    TemplateLoadStatusString = "Loading template";
                    break;
                case (int)ScratchPadConstants.LoadStatus.LoadFinished:
                    TemplateLoadStatusString = "Finished loading template";
                    break;
                default:
                    TemplateLoadStatusString = "Off";
                    break;
            }
        }

        public OptoMonitorViewModel()
        {
            SetStrategyLocationString();
            SetTemplateStatusString();
        }
    }
}
