using System;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class DynoPidRampDisplayViewModel : BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private decimal _currentSetpoint { get; set; }
        public decimal CurrentSetpoint { get {  return _currentSetpoint; } set { _currentSetpoint = value; OnPropertyChanged("CurrentSetpoint"); } }

        private string _currentTime { get; set; }
        public string CurrentTime { get { return _currentTime; } set { _currentTime = value; OnPropertyChanged("CurrentTime"); } }

        public decimal Setpoint { get; set; }

        public long Time { get; set; }

        public WidgetConstants.DynoPidMeasurement PidMode { get; set; }

        public DynoPidRampDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint))
                Setpoint = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint))
                Time = Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampMode))
                PidMode = (WidgetConstants.DynoPidMeasurement)Convert.ToInt32(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampMode).Value);
            else
                PidMode = WidgetConstants.DynoPidMeasurement.Rpm;
        }
    }
}
