using System;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class StarterDisplayViewModel: BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private bool _isCranking { get; set; }
        public bool IsCranking
        {
            get { return _isCranking; }
            set { _isCranking = value; OnPropertyChanged("IsCranking"); }
        }

        private decimal _engineRpm { get; set; }
        public decimal EngineRpm
        {
            get { return _engineRpm; }
            set { _engineRpm = value; OnPropertyChanged("EngineRpm"); }
        }

        private decimal _startRpm { get; set; }
        public decimal StartRpm
        {
            get { return _startRpm; }
            set { _startRpm = value; OnPropertyChanged("StartRpm"); }
        }

        private int _attempt { get; set; }
        public int Attempt
        {
            get { return _attempt; }
            set { _attempt = value; OnPropertyChanged("Attempt"); }
        }

        public decimal CrankTime { get; set; }
        public int RetryCount { get; set; }
        public decimal TimeBetweenRetries { get; set; }
        public decimal CutoffTime { get; set; }
        public decimal Timeout { get; set; }

        public StarterDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterStartRpm))
                StartRpm = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterStartRpm).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterCrankTime))
                CrankTime = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterCrankTime).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterNumberOfTries))
                RetryCount = Convert.ToInt32(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterNumberOfTries).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeBetweenTries))
                TimeBetweenRetries = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeBetweenTries).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeAtRpm))
                CutoffTime = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeAtRpm).Value);
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeout))
                Timeout = Convert.ToDecimal(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeout).Value);
        }
    }
}
