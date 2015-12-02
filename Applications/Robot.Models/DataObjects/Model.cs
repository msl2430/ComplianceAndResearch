
using System.Collections.Generic;

namespace Robot.Models.DataObjects
{
    public class Model
    {
        public virtual int ModelId { get; set; }
        public virtual int ManufacturerId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Year { get; set; }
        public virtual decimal? PulseMultiplier { get; set; }
        public virtual decimal? PulsePerRev { get; set; }
    }

    public class ModelExtended
    {
        public virtual int ModelId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual string Name { get; set; }
        public virtual int Year { get; set; }
        public virtual decimal? PulseMultiplier { get; set; }
        public virtual decimal? PulsePerRev { get; set; }
        public virtual IList<GearRatio> GearRatios { get; set; }
    }
}
