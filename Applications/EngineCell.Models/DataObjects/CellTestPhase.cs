using System.Collections.Generic;

namespace EngineCell.Models.DataObjects
{
    public class CellTestPhase
    {
        public virtual int CellTestPhaseId { get; set; }
        public virtual int CellTestId { get; set; }
        public virtual int PhaseOrder { get; set; }
        public virtual string Name { get; set; }
    }

    public class CellTestPhaseExtended
    {
        public virtual int CellTestPhaseId { get; set; }
        public virtual int CellTestId { get; set; }
        public virtual int PhaseOrder { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<CellTestPhaseWidgetExtended> Widgets { get; set; }
        public virtual IList<CellTestPhaseTriggerExtended> Triggers { get; set; }
    }
}
