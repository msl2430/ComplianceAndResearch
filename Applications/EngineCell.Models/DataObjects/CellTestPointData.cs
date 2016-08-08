using System;

namespace EngineCell.Models.DataObjects
{
    //dbo.CellTestPointData
    public class CellTestPointData
    {
        public virtual int CellTestPointDataId { get; set; }
        public virtual int CellTestId { get; set; }
        public virtual int CellPointId { get; set; }
        public virtual decimal Data { get; set; }
        public virtual DateTime CaptureTime { get; set; }
    }

    //dbo.CellTestPointData
    public class CellTestPointDataExtended
    {
        public virtual int CellTestPointDataId { get; set; }
        public virtual CellTest CellTest { get; set; }
        public virtual CellPoint CellPoint { get; set; }
        public virtual decimal Data { get; set; }
        public virtual DateTime CaptureTime { get; set; }
    }
}
