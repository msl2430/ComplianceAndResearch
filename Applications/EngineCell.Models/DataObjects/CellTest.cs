using System;
using System.Collections.Generic;

namespace EngineCell.Models.DataObjects
{
    //dbo.CellTest
    public class CellTest
    {
        public virtual int CellTestId { get; set; }
        public virtual int CellId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
    }

    public class CellTestExtended
    {
        public virtual int CellTestId { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
        public virtual IList<CellTestPhaseExtended> Phases { get; set; }
    }
}
