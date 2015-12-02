using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    public class ManufacturerModel : BaseModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string DisplayName { get; set; }

        public ManufacturerModel(ManufacturerExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);

            CountryId = obj.Country.CountryId;
            CountryName = obj.Country.Name;
            DisplayName = string.Format("{0} ({1})", Name, CountryName);
        }
    }
}
