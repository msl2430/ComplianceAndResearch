using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    public class CarModel : BaseModel
    {
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int ManufactureId { get; set; }

        public CarModel() { }

        public CarModel(Model obj)
        {
            if (obj == null)
                return;
            InstantiateFromDataObject(obj);
        }
    }
}
