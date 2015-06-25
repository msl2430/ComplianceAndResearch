using Robot.Core.Models;
using Robot.Models.DataObjects;

namespace Robot.Models.Models
{
    public class CountryModel : BaseModel
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public CountryModel(Country obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
