using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class CellTestPhaseWidgetModel : BaseModel
    {
        public int CellTestPhaseWidgetId { get; set; }
        public int CellTestPhaseId { get; set; }
        public int WidgetId { get; set; }
        public WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public string Value { get; set; }
        public IList<PhaseEndSettingModel> PhaseEndSettings { get; set; }

        public CellTestPhaseWidgetModel()
        {
            PhaseEndSettings = new List<PhaseEndSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidget obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            PhaseEndSettings = new List<PhaseEndSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidgetExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            PhaseEndSettings = obj.PhaseEndSettings.IsNotNullOrEmpty() ? obj.PhaseEndSettings.Select(p => new PhaseEndSettingModel(p)).ToList() : new List<PhaseEndSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidget obj, IList<CellTestPhaseWidgetPhaseEndSetting> phaseEnd)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            PhaseEndSettings = phaseEnd.IsNotNullOrEmpty() ? phaseEnd.Select(p => new PhaseEndSettingModel(p)).ToList() : new List<PhaseEndSettingModel>();
        }
    }
}
