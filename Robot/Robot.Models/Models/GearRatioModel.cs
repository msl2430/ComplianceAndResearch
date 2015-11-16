using Robot.Core.Models;
using Robot.Models.DataObjects;
using System;

namespace Robot.Models.Models
{
    [Serializable]
    public class GearRatioModel : BaseModel
    {
        public int GearRatioId { get; set; }
        public int ModelId { get; set; }
        public int Gear { get; set; }
        public decimal Ratio { get; set; }
        public string GearName => "Gear " + Gear;

        public GearRatioModel() { }
        public GearRatioModel(GearRatio obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
