using System;

namespace EngineCell.Models.Models
{
    public class CellTestPointDataModel
    {
        public int CellTestPointDataId { get; set; }
        public int CellTestId { get; set; }
        public int CellPointId { get; set; }
        public decimal Data { get; set; }
        public DateTime CaptureDatTime { get; set; }
    }
}
