using System;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class CellTestPhaseTriggerModel : BaseModel
    {
        public int CellTestPhaseTriggerId { get; set; }
        public int CellTestPhaseId { get; set; }
        public int CellPointId { get; set; }
        public string CellPointName { get; set; }
        public decimal LowValue { get; set; }
        public decimal HighValue { get; set; }
        public int? SecondsThreshold { get; set; }
        public WidgetConstants.TriggerResultType ResultTypeId { get; set; }
        public string ResultTypeParameter { get; set; }
        public bool IsAlarm { get; set; }
        public bool IsEmail { get; set; }

        public CellTestPhaseTriggerModel() { }

        public CellTestPhaseTriggerModel(CellTestPhaseTrigger obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }

        public CellTestPhaseTriggerModel(CellTestPhaseTriggerExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            CellPointId = obj.CellPoint.CellPointId;
            CellPointName = obj.CellPoint.CustomName;
        }
    }
}
