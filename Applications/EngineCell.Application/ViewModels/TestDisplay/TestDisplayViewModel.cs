using System.Collections.ObjectModel;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.ViewModels.TestDisplay
{
    public class TestDisplayViewModel : BaseViewModel
    {
        private DynoThrottlePidViewModel _dynoThrottlePid { get; set; }
        public DynoThrottlePidViewModel DynoThrottlePid {
            get { return _dynoThrottlePid; }
            set { _dynoThrottlePid = value; OnPropertyChanged("DynoThrottlePid"); }
        }

        public PidDisplayModel DynoPid1 { get; set; }

        public PidDisplayModel DynoPid2 { get; set; }

        public PidDisplayModel ThrottlePid1 { get; set; }

        public PidDisplayModel ThrottlePid2 { get; set; }

        public PidDisplayViewModel CoolantPid { get; set; }

        public PidDisplayViewModel OilPid { get; set; }

        public PidDisplayViewModel Intercooler { get; set; }

        private VentilationControlDisplayViewModel _ventControl1Display { get; set; }
        public VentilationControlDisplayViewModel VentControl1Display {
            get { return _ventControl1Display; }
            set { _ventControl1Display = value; OnPropertyChanged("VentControl1Display"); }
        }

        private DynoPidDisplayViewModel _dynoPidDisplay { get; set; }
        public DynoPidDisplayViewModel DynoPidDisplay
        {
            get { return _dynoPidDisplay; }
            set { _dynoPidDisplay = value; OnPropertyChanged("DynoPidDisplay"); }
        }

        private bool _phaseStarted { get; set; }
        public bool PhaseStarted { get { return _phaseStarted; } set { _phaseStarted = value; OnPropertyChanged("PhaseStarted"); } }

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        private ObservableCollection<PointDataModel> _visiblePoints { get; set; }
        public ObservableCollection<PointDataModel> VisiblePoints {
            get { return _visiblePoints; }
            set { _visiblePoints = value; OnPropertyChanged("VisiblePoints"); }
        }

        private ObservableCollection<PointDataModel> _leftVisiblePoints { get; set; }
        public ObservableCollection<PointDataModel> LeftVisiblePoints
        {
            get { return _leftVisiblePoints; }
            set { _leftVisiblePoints = value; OnPropertyChanged("LeftVisiblePoints"); }
        }

        private ObservableCollection<PointDataModel> _rightVisiblePoints { get; set; }
        public ObservableCollection<PointDataModel> RightVisiblePoints
        {
            get { return _rightVisiblePoints; }
            set { _rightVisiblePoints = value; OnPropertyChanged("RightVisiblePoints"); }
        }

        private bool _isManualTest { get; set; }
        public bool IsManualTest {
            get { return _isManualTest; }
            set { _isManualTest = value; OnPropertyChanged("IsManualTest"); }
        }

        public IPointWorkerService PointWorkerService { get; set; }

        public TestDisplayViewModel(IApplicationSessionFactory appSession, StripChartViewModel chartViewModel)
        {
            ZIndex = 1;
            PhaseStarted = false;
            ApplicationSessionFactory = appSession;
            DynoThrottlePid = new DynoThrottlePidViewModel()
            {
                Dyno = new PidDisplayModel("Dyno") {AutoValue = 123, SettingAuto = ControlConstants.PidSetting.Speed },
                Throttle = new PidDisplayModel("Throttle") { SettingManual = ControlConstants.PidSetting.Direct },
                ApplicationSessionFactory = appSession
            };
            CoolantPid = new PidDisplayViewModel() {PidConfig = new PidDisplayModel("Coolant"), PidType = ControlConstants.PidType.Coolant, ApplicationSessionFactory = appSession };
            OilPid = new PidDisplayViewModel() { PidConfig = new PidDisplayModel("Oil"), PidType = ControlConstants.PidType.Oil, ApplicationSessionFactory = appSession };
            Intercooler = new PidDisplayViewModel() { PidConfig = new PidDisplayModel("Intercooler"), PidType = ControlConstants.PidType.Intercooler, ApplicationSessionFactory = appSession };
            IsManualTest = false;
            ChartViewModel = chartViewModel;            
            VisiblePoints = new ObservableCollection<PointDataModel>();
            LeftVisiblePoints = new ObservableCollection<PointDataModel>();
            RightVisiblePoints = new ObservableCollection<PointDataModel>();
            UpdateVisibleCellPoints();
        }

        public void UpdateVisibleCellPoints()
        {
            VisiblePoints.Clear();
            foreach (var point in ApplicationSessionFactory.CellPoints.Where(cp => cp.IsAverage))
            {
                VisiblePoints.Add(point);
            }

            LeftVisiblePoints.Clear();
            RightVisiblePoints.Clear();

            for (var i = 0; i < VisiblePoints.Count; i++)
            {
                if (i%2 == 0)
                    LeftVisiblePoints.Add(VisiblePoints[i]);
                else
                    RightVisiblePoints.Add(VisiblePoints[i]);
            }
        }
    }
}
