
namespace Robot.Models.DataObjects
{
    public class Manufacturer
    {
        public virtual int ManufacturerId { get; set; }
        public virtual string Name { get; set; }
        public virtual int CountryId { get; set; } 
    }

    public class ManufacturerExtended
    {
        public virtual int ManufacturerId { get; set; }
        public virtual string Name { get; set; }
        public virtual Country Country { get; set; }
    }
}
