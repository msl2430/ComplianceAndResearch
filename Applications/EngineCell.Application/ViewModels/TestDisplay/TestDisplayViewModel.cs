using System.Collections;
using System.Collections.Generic;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Pid;
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

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        public TestDisplayViewModel(IApplicationSessionFactory appSession, StripChartViewModel chartViewModel)
        {
            ZIndex = 1;
            ApplicationSessionFactory = appSession;
            DynoThrottlePid = new DynoThrottlePidViewModel()
            {
                Dyno = new PidDisplayModel("Dyno") {AutoValue = 123, SettingAuto = ControlConstants.PidSetting.Speed },
                Throttle = new PidDisplayModel("Throttle") { SettingManual = ControlConstants.PidSetting.Direct }
            };
            DynoPid1 = new PidDisplayModel("Dyno") { AutoValue = 123 };            
            //DynoPid2 = new PidDisplay("Dyno 2");
            ThrottlePid1 = new PidDisplayModel("Throttle");
            //ThrottlePid2 = new PidDisplay("Throttle 2");
            CoolantPid = new PidDisplayViewModel() {PidConfig = new PidDisplayModel("Coolant")};
            OilPid = new PidDisplayViewModel() { PidConfig = new PidDisplayModel("Oil")};
            Intercooler = new PidDisplayViewModel() { PidConfig = new PidDisplayModel("Intercooler")};
            ChartViewModel = chartViewModel;
        }
    }
}
