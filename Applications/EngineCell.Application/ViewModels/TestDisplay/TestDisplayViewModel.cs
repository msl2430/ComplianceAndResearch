using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.StripChart;

namespace EngineCell.Application.ViewModels.TestDisplay
{
    public class TestDisplayViewModel : BaseViewModel
    {
        public PidSetting DynoPid1 { get; set; }

        public PidSetting DynoPid2 { get; set; }

        public PidSetting ThrottlePid1 { get; set; }

        public PidSetting ThrottlePid2 { get; set; }

        public PidSetting CoolantPid { get; set; }

        public PidSetting OilPid { get; set; }

        public PidSetting Intercooler { get; set; }

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        public TestDisplayViewModel(IApplicationSessionFactory appSession, StripChartViewModel chartViewModel)
        {
            ZIndex = 1;
            ApplicationSessionFactory = appSession;
            DynoPid1 = new PidSetting("Dyno 1");
            DynoPid2 = new PidSetting("Dyno 2");
            ThrottlePid1 = new PidSetting("Throttle 1");
            ThrottlePid2 = new PidSetting("Throttle 2");
            CoolantPid = new PidSetting("Coolant");
            OilPid = new PidSetting("Oil");
            Intercooler = new PidSetting("Intercooler");
            ChartViewModel = chartViewModel;
        }
    }
}
