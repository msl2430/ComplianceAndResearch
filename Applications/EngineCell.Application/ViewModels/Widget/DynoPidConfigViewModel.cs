using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class DynoPidConfigViewModel : BaseWidgetViewModel
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

        public DynoPidConfigViewModel()
        {
            DynoMode = WidgetConstants.DynoPidMode.Motor;
            DynoMeasurement = WidgetConstants.DynoPidMeasurement.Rpm;
            DynoSetpoint = 0m;
            IsActive = false;
        }

        public override void SetValues(IList<WidgetSettingValueModel> settings)
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

        public override IList<WidgetSettingValueModel> GetValues(int cellId)
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
