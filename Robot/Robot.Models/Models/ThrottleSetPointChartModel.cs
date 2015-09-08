using Robot.Core.Constants;
using Robot.Core.Models;
using Robot.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Models.Models
{
    [Serializable]
    public class ThrottleSetPointChartModel : BaseModel
    {
        public int ThrottleSetPointChartId { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public StatusConstants.SqlStatus Status { get; set; }
        public DateTime CreationDateTime { get; set; }
        public IList<ThrottleSetPointModel> SetPoints { get; set; }

        public ThrottleSetPointChartModel() { }

        public ThrottleSetPointChartModel(ThrottleSetPointChart obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            SetPoints = new List<ThrottleSetPointModel>();
        }

        public ThrottleSetPointChartModel(ThrottleSetPointChartExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            SetPoints = obj.SetPoints.Select(sp => new ThrottleSetPointModel(sp)).ToList();
        }
    }
}
