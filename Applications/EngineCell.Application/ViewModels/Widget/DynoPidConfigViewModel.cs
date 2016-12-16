using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class DynoPidConfigViewModel : BaseViewModel
    {
        private WidgetConstants.DynoPidMode _dynoMode { get; set; }
        public WidgetConstants.DynoPidMode DynoMode
        {
            get { return _dynoMode;}
            set { _dynoMode = value; OnPropertyChanged("DynoMode"); }
        }

        private WidgetConstants.DynoPidMeasurement _dynoMeasurement { get; set; }
        public WidgetConstants.DynoPidMeasurement DynoMeasurement
        {
            get { return _dynoMeasurement; }
            set { _dynoMeasurement = value; OnPropertyChanged("DynoMeasurement"); }
        }

        private decimal _dynoSetpoint { get; set; }
        public decimal DynoSetpoint
        {
            get { return _dynoSetpoint; }
            set { _dynoSetpoint = value; OnPropertyChanged("DynoSetpoint"); }
        }

        private bool _isActive { get; set; }
        public bool IsActive { get { return _isActive; } set { _isActive = value; OnPropertyChanged("IsActive"); } }

        public DynoPidConfigViewModel()
        {
            DynoMode = WidgetConstants.DynoPidMode.Motor;
            DynoMeasurement = WidgetConstants.DynoPidMeasurement.Rpm;
            DynoSetpoint = 0m;
            IsActive = false;
        }

        public void SetValues(IList<WidgetSettingValueModel> settings)
        {
            foreach (var setting in settings)
            {
                switch (setting.WidgetSettingId)
                {
                    case WidgetConstants.WidgetSetting.DynoPidMode:
                        DynoMode = (WidgetConstants.DynoPidMode)Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.DynoPidMeasurement:
                        DynoMeasurement = (WidgetConstants.DynoPidMeasurement)Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.DynoPidSetpoint:
                        DynoSetpoint = Convert.ToDecimal(setting.Value);
                        break;                    
                }
            }
        }

        public IList<WidgetSettingValueModel> GetValues(int cellId)
        {
            return new List<WidgetSettingValueModel>
            {
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.DynoPid, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.DynoPidMode, Value = DynoMode.ToInt().ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.DynoPid, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.DynoPidMeasurement, Value = DynoMeasurement.ToInt().ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.DynoPid, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.DynoPidSetpoint, Value = DynoSetpoint.ToString()},
            };
        }
    }
}
