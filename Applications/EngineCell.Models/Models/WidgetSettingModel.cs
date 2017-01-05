using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class WidgetSettingModel : BaseModel
    {
        public int CellTestPhaseWidgetSettingId { get; set; }
        public int CellTestPhaseWidgetId { get; set; }
        public WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public string Value { get; set; }

        //Test Schedule Widget
        public IList<ScheduleData> Schedule { get; set; }

        public WidgetSettingModel() { }

        public WidgetSettingModel(CellTestPhaseWidgetSetting obj)
        {
            if (obj == null)
                return;
            InstantiateFromDataObject(obj);
        }
    }
}
