using Robot.Core.Constants;
using Robot.Core.Models;
using Robot.Models.DataObjects;
using System;
using System.Collections.Generic;

namespace Robot.Models.Models
{
    [Serializable]
    public class RoadTestChartModel : BaseModel
    {
        public int RoadTestChartId { get; set; }
        public string Name { get; set; }
        public StatusConstants.SqlStatus Status { get; set; }
        public IList<RoadTestPoint> RoadTestPoints { get; set; }

        public RoadTestChartModel() { }

        public RoadTestChartModel(RoadTestChart obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);

            RoadTestPoints = new List<RoadTestPoint>();
        }

        public RoadTestChartModel(RoadTestChartExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);

            RoadTestPoints = obj.RoadTestPoints ?? new List<RoadTestPoint>();
        }
    }
}
