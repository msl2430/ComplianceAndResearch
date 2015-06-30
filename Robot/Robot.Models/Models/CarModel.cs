using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    public class CarModel : BaseModel
    {
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int ManufactureId { get; set; }
        public int Year { get; set; }
        public string DisplayName { get; set; }

        public CarModel() { }

        public CarModel(Model obj)
        {
            if (obj == null)
                return;
            InstantiateFromDataObject(obj);
            DisplayName = string.Format("{0} ({1})", Name, Year);
        }
    }
}
