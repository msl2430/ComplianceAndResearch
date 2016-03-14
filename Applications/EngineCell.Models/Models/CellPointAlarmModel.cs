using System;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    public class CellPointAlarmModel : BaseModel
    {
        public int CellPointAlarmId { get; set; }
        public int CellPointId { get; set; }
        public decimal LowShutdownWarning { get; set; }
        public decimal LowShutdownExecute { get; set; }
        public decimal HighShutdownWarning { get; set; }
        public decimal HighShutdownExecute { get; set; }
        public DateTime UpdateDateTime { get; set; }

        public CellPointAlarmModel() { }

        public CellPointAlarmModel(CellPointAlarm obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
