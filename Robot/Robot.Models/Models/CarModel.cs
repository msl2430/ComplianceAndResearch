using System.Collections.Generic;
using System.Linq;
using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    public class CarModel : BaseModel
    {
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        public int Year { get; set; }
        public string DisplayName { get; set; }
        public decimal? PulseMultiplier { get; set; }
        public decimal? PulsePerRev { get; set; }
        public IList<GearRatioModel> GearRatios { get; set; }

        public CarModel()
        {
            GearRatios = new List<GearRatioModel>();
        }

        public CarModel(Model obj)
        {
            if (obj == null)
                return;
            InstantiateFromDataObject(obj);
            GearRatios = new List<GearRatioModel>();
            DisplayName = $"{Name} ({Year})";
        }

        public CarModel(ModelExtended obj)
        {
            if (obj == null)
                return;
            InstantiateFromDataObject(obj);
            DisplayName = $"{Name} ({Year})";
            GearRatios = obj.GearRatios != null && obj.GearRatios.Any() ? obj.GearRatios.Select(gr => new GearRatioModel(gr)).ToList() : new List<GearRatioModel>();
        }
    }
}
