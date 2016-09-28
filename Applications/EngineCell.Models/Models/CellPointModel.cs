using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    public class CellPointModel : BaseModel
    {
        public int CellPointId { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; }
        public int PointId { get; set; }
        public string PointName { get; set; }
        public bool IsInput { get; set; }
        public bool IsAnalog { get; set; }
        public CellPointAlarmModel Alarm { get; set; }
        public PointGroup PointGroupId { get; set; }
        public string CustomName { get; set; }
        public bool IsRecord { get; set; }
        public bool IsAverage { get; set; }
        public int? AverageSeconds { get; set; }
        public bool IncludeInStripChart { get; set; }
        public decimal? StripChartScale { get; set; }
        public bool IsCustomValue { get; set; }
        public decimal? CustomValue { get; set; }

        public CellPointModel() { }

        public CellPointModel(CellPointExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);

            CellId = obj.Cell.CellId;
            CellName = obj.Cell.Name;
            PointId = obj.Point.PointId;
            PointName = obj.Point.Name;
            IsAnalog = obj.Point.IsAnalog;
            IsInput = obj.Point.IsInput;
            PointGroupId = obj.Point.PointGroupId;
            Alarm = new CellPointAlarmModel(obj.Alarm);
        }
    }
}
