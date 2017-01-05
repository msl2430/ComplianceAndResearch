using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    public class StarterConfigViewModel : BaseWidgetViewModel
    {
        private int _crankTime { get; set; }
        public int CrankTime
        {
            get {  return _crankTime;}
            set { _crankTime = value; OnPropertyChanged("CrankTime"); }
        }

        private decimal _startParameter { get; set; }
        public decimal StartParameter
        {
            get { return _startParameter; }
            set { _startParameter = value; OnPropertyChanged("StartParameter"); }
        }

        private int _addedCrankTime { get; set; }
        public int AddedCrankTime
        {
            get { return _addedCrankTime; }
            set { _addedCrankTime = value; OnPropertyChanged("AddedCrankTime"); }
        }

        public StarterConfigViewModel()
        {
            CrankTime = 10;
            StartParameter = 1;
            AddedCrankTime = 10;
            IsActive = false;
        }

        public override void SetValues(IList<WidgetSettingValueModel> settings)
        {
            foreach (var setting in settings)
            {
                switch (setting.WidgetSettingId)
                {
                    case WidgetConstants.WidgetSetting.StarterInitialCrankTime:
                        CrankTime = Convert.ToInt32(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.StarterStartParameter:
                        StartParameter = Convert.ToDecimal(setting.Value);
                        break;
                    case WidgetConstants.WidgetSetting.StarterAddedCrankTime:
                        AddedCrankTime = Convert.ToInt32(setting.Value);
                        break;
                }
            }
        }

        public override IList<WidgetSettingValueModel> GetValues(int cellId)
        {
            return new List<WidgetSettingValueModel>
            {
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.Starter, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.StarterInitialCrankTime, Value = CrankTime.ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.Starter, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.StarterStartParameter, Value = StartParameter.ToString()},
                new WidgetSettingValueModel {WidgetId = WidgetConstants.Widget.Starter, CellId = cellId, WidgetSettingId = WidgetConstants.WidgetSetting.StarterAddedCrankTime, Value = AddedCrankTime.ToString()},
            };
        }
    }
}
