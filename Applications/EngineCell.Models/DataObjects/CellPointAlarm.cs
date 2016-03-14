using System;

namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.PointAlarm
    /// </summary>
    public class CellPointAlarm
    {
        public virtual int CellPointAlarmId { get; set; }
        public virtual int CellPointId { get; set; }
        public virtual decimal LowShutdownWarning { get; set; }
        public virtual decimal LowShutdownExecute { get; set; }
        public virtual decimal HighShutdownWarning { get; set; }
        public virtual decimal HighShutdownExecute { get; set; }
        public virtual DateTime UpdateDateTime { get; set; }
    }
}
