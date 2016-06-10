using System.Collections.ObjectModel;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Constants;

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

        private bool _phaseStarted { get; set; }
        public bool PhaseStarted { get { return _phaseStarted; } set { _phaseStarted = value; OnPropertyChanged("PhaseStarted"); } }

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        private ObservableCollection<PointDataModel> _visiblePoints { get; set; }
        public ObservableCollection<PointDataModel> VisiblePoints {
            get { return _visiblePoints; }
            set { _visiblePoints = value; OnPropertyChanged("VisiblePoints"); }
        }

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
            ChartViewModel = chartViewModel;
            UpdateVisibleCellPoints();
        }

        public void UpdateVisibleCellPoints()
        {
            VisiblePoints = new ObservableCollection<PointDataModel>();
            foreach (var point in ApplicationSessionFactory.CellPoints.Where(cp => cp.IsAverage))
            {
                VisiblePoints.Add(point);
            }
        }
    }
}
