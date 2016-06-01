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
        public virtual DateTime CaptureDatTime { get; set; }
    }
}
