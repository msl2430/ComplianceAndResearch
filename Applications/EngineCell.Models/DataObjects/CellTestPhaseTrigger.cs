using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    public class CellTestPhaseTrigger
    {
        public virtual int CellTestPhaseTriggerId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual int CellPointId { get; set; }
        public virtual decimal LowValue { get; set; }
        public virtual decimal HighValue { get; set; }
        public virtual int SecondsThreshold { get; set; }
        public virtual WidgetConstants.TriggerResultType ResultTypeId { get; set; }
        public virtual string ResultTypeParameter { get; set; }
        public virtual bool IsAlarm { get; set; }
        public virtual bool IsEmail { get; set; }
    }

    public class CellTestPhaseTriggerExtended
    {
        public virtual int CellTestPhaseTriggerId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual CellPoint CellPoint { get; set; }
        public virtual decimal LowValue { get; set; }
        public virtual decimal HighValue { get; set; }
        public virtual int SecondsThreshold { get; set; }
        public virtual WidgetConstants.TriggerResultType ResultTypeId { get; set; }
        public virtual string ResultTypeParameter { get; set; }
        public virtual bool IsAlarm { get; set; }
        public virtual bool IsEmail { get; set; }
    }
}
