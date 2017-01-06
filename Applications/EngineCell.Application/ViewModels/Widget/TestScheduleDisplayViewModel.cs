using System.Collections.Generic;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class TestScheduleDisplayViewModel : BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private decimal _dynoSetpoint { get; set; }
        public decimal DynoSetpoint { get {  return _dynoSetpoint; } set { _dynoSetpoint = value; OnPropertyChanged("DynoSetpoint"); } }

        private decimal _throttleSetpoint { get; set; }
        public decimal ThrottleSetpoint { get { return _throttleSetpoint; } set { _throttleSetpoint = value; OnPropertyChanged("ThrottleSetpoint"); } }

        public IList<KeyValuePair<WidgetConstants.TestScheduleSetpointType, IList<ScheduleData>>> CalculatedScheduleData { get; set; }
        public bool HasDyno { get; set; }
        public bool HasThrottle { get; set; }

        private string _testRunTime { get; set; }
        public string TestRunTime { get { return _testRunTime; } set { _testRunTime = value; OnPropertyChanged("TestRunTime"); } }

        public TestScheduleDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            TestRunTime = "00:00";
        }
    }
}
