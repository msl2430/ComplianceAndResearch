using System.Collections;
using System.Collections.Generic;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Application.ViewModels.StripChart;

namespace EngineCell.Application.ViewModels.TestDisplay
{
    public class TestDisplayViewModel : BaseViewModel
    {
        public PidDisplayModel DynoPid1 { get; set; }

        public PidDisplayModel DynoPid2 { get; set; }

        public PidDisplayModel ThrottlePid1 { get; set; }

        public PidDisplayModel ThrottlePid2 { get; set; }

        public PidDisplayModel CoolantPid { get; set; }

        public PidDisplayModel OilPid { get; set; }

        public PidDisplayModel Intercooler { get; set; }

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        public TestDisplayViewModel(IApplicationSessionFactory appSession, StripChartViewModel chartViewModel)
        {
            ZIndex = 1;
            ApplicationSessionFactory = appSession;
            DynoPid1 = new PidDisplayModel("Dyno") { AutoValue = 123 };            
            //DynoPid2 = new PidDisplay("Dyno 2");
            ThrottlePid1 = new PidDisplayModel("Throttle");
            //ThrottlePid2 = new PidDisplay("Throttle 2");
            CoolantPid = new PidDisplayModel("Coolant");
            OilPid = new PidDisplayModel("Oil");
            Intercooler = new PidDisplayModel("Intercooler");
            ChartViewModel = chartViewModel;
        }
    }
}
