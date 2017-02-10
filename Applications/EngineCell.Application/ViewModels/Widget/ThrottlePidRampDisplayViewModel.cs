using System;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class ThrottlePidRampDisplayViewModel : BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private decimal _currentSetpoint { get; set; }
        public decimal CurrentSetpoint { get { return _currentSetpoint; } set { _currentSetpoint = value; OnPropertyChanged("CurrentSetpoint"); } }

        private string _currentTime { get; set; }
        public string CurrentTime { get { return _currentTime; } set { _currentTime = value; OnPropertyChanged("CurrentTime"); } }

        public decimal Setpoint { get; set; }

        public long Time { get; set; }

        public WidgetConstants.DynoPidMeasurement PidMode { get; set; }

        public ThrottlePidRampDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint))
                Setpoint = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint))
                Time = Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode))
                PidMode = (WidgetConstants.DynoPidMeasurement)Convert.ToInt32(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode).Value);
            else 
                PidMode = WidgetConstants.DynoPidMeasurement.Rpm;
        }
    }
}
