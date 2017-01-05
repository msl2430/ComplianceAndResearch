using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class VentilationControlViewModel : BaseWidgetViewModel
    {
        private ThermoCouple _inside { get; set; }
        public ThermoCouple Inside
        {
            get { return _inside; }
            set { _inside = value; OnPropertyChanged("Inside"); }
        }

        private ThermoCouple _outside { get; set; }
        public ThermoCouple Outside
        {
            get { return _outside; }
            set { _outside = value; OnPropertyChanged("Outside"); }
        }

        private AnalogOutput _output { get; set; }
        public AnalogOutput Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged("Output"); }
        }

        private decimal _gain { get; set; }
        public decimal Gain
        {
            get { return _gain; }
            set { _gain = value; OnPropertyChanged("Gain"); }
        }

        private decimal _setPoint { get; set; }
        public decimal SetPoint
        {
            get { return _setPoint; }
            set { _setPoint = value; OnPropertyChanged("SetPoint"); }
        }

        public VentilationControlViewModel()
        {
            Inside = ThermoCouple.ThermoCouple1;
            Outside = ThermoCouple.ThermoCouple2;
            Output = AnalogOutput.Ao10Bipolar;
            IsActive = false;
        }

        public override void SetValues(IList<WidgetSettingValueModel> settings)
        {
            foreach (var setting in settings)
            {
                switch (setting.WidgetSettingId)
                {
                    case WidgetConstants.WidgetSetting.VentCtrl1InsideThermoCouple:
                        Inside = (ThermoCouple)Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.VentCtrl1OutsideThermoCouple:
                        Outside = (ThermoCouple)Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.VentCtrl1AnalogOutput:
                        Output = (AnalogOutput)Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.VentCtrl1Gain:
                        Gain = Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.VentCtrl1SetPoint:
                        SetPoint = Convert.ToInt32(setting.Value);
                        break;
                }
            }
        }

        public override IList<WidgetSettingValueModel> GetValues(int cellId)
        {
            return new List<WidgetSettingValueModel>
            {
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.VentilationControl1, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.VentCtrl1InsideThermoCouple, Value = Inside.ToInt().ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.VentilationControl1, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.VentCtrl1OutsideThermoCouple, Value = Outside.ToInt().ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.VentilationControl1, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.VentCtrl1AnalogOutput, Value = Output.ToInt().ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.VentilationControl1, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.VentCtrl1Gain, Value = Gain.ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.VentilationControl1, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.VentCtrl1SetPoint, Value = SetPoint.ToString()},
            };
        }
    }
}
