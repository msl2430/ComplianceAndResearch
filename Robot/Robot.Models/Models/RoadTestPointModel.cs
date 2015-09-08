using Robot.Core.Models;
using System;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    [Serializable]
    public class RoadTestPointModel : BaseModel
    {
        public int RoadTestPointId { get; set; }
        public int RoadTestChartId { get; set; }
        public decimal SecondFromStart { get; set; }
        public decimal Speed { get; set; }

        public RoadTestPointModel() { }

        public RoadTestPointModel(RoadTestPoint obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
