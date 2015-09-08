using System;
using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    [Serializable]
    public class ThrottleSetPointModel : BaseModel
    {
        public int ThrottleSetPointId { get; set; }
        public int ThrottleSetPointChartId { get; set; }
        public int Speed { get; set; }
        public int Gear { get; set; }
        public decimal Acceleration { get; set; }
        public decimal SetPoint { get; set; }

        public ThrottleSetPointModel() { }

        public ThrottleSetPointModel(ThrottleSetPoint obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
