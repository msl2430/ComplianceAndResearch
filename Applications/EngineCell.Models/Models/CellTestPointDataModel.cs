using System;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    public class CellTestPointDataModel : BaseModel
    {
        public int CellTestPointDataId { get; set; }
        public int CellTestId { get; set; }
        public DateTime? CellTestStart { get; set; }
        public DateTime? CellTestEnd { get; set; }
        public int CellPointId { get; set; }
        public string CellPointName { get; set; }
        public decimal Data { get; set; }
        public DateTime CaptureTime { get; set; }

        public CellTestPointDataModel() { }

        public CellTestPointDataModel(CellTestPointDataExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);

            CellTestId = obj.CellTest.CellTestId;
            CellTestStart = obj.CellTest.StartTime == null ? (DateTime?)null : Convert.ToDateTime(obj.CellTest.StartTime);
            CellTestEnd = obj.CellTest.EndTime == null ? (DateTime?)null : Convert.ToDateTime(obj.CellTest.EndTime);
            CellPointId = obj.CellPoint.CellPointId;
            CellPointName = obj.CellPoint.CustomName;
        }
    }
}
