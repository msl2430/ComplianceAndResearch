using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class ModelMap :ClassMap<Model>
    {
        public ModelMap()
        {
            Table("Model");
            Cache.NonStrictReadWrite();

            Id(m => m.ModelId);

            Map(m => m.ManufacturerId);
            Map(m => m.Name);
            Map(m => m.Year);
            Map(m => m.PulseMultiplier);
            Map(m => m.PulsePerRev);
        }
    }

    internal sealed class ModelExtendedMap : ClassMap<ModelExtended>
    {
        public ModelExtendedMap()
        {
            Table("Model");
            Cache.NonStrictReadWrite();

            Id(m => m.ModelId);

            Map(m => m.Name);
            Map(m => m.Year);
            Map(m => m.PulseMultiplier);
            Map(m => m.PulsePerRev);

            References(m => m.Manufacturer)
                .Column("ManufacturerId")
                .Not.Nullable()
                .Fetch.Join();

            HasMany(m => m.GearRatios)
                .KeyColumn("ModelId")
                .Fetch.Subselect();
        }
    }
}
