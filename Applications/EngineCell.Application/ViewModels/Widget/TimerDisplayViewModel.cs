using System;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class TimerDisplayViewModel : BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private string _currentTime { get; set; }
        public string CurrentTime { get { return _currentTime; } set { _currentTime = value; OnPropertyChanged("CurrentTime"); } }

        public string ExpirationTime { get; set; }

        public long ExpirationTimeSeconds { get; set; }

        public TimerDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration))
            {
                ExpirationTimeSeconds = Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration).Value);
                var timespan = new TimeSpan(0, 0, 0, Convert.ToInt32(ExpirationTimeSeconds));
                ExpirationTime = timespan.Days.ToString("0#") + ":" + timespan.Hours.ToString("0#") + ":" + timespan.Minutes.ToString("0#") + ":" + timespan.Seconds.ToString("0#");
            }
        }
    }
}
