using System;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class PhaseEndSettingModel : BaseModel
    {
        public int CellTestPhaseWidgetPhaseEndSettingId { get; set; }
        public int CellTestPhaseWidgetId { get; set; }
        public WidgetConstants.PhaseEndSetting PhaseEndSettingId { get; set; }
        public string Value { get; set; }

        public PhaseEndSettingModel() { }

        public PhaseEndSettingModel(CellTestPhaseWidgetPhaseEndSetting obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
