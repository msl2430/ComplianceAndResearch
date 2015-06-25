using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class ManufacturerMap : ClassMap<Manufacturer>
    {
        public ManufacturerMap()
        {
            Table("Manufacturer");
            Cache.NonStrictReadWrite();

            Id(m => m.ManufacturerId);

            Map(m => m.Name);
            Map(m => m.CountryId);
        }
    }

    internal sealed class ManufacturerExtendedMap : ClassMap<ManufacturerExtended>
    {
        public ManufacturerExtendedMap()
        {
            Table("Manufacturer");
            Cache.NonStrictReadWrite();

            Id(m => m.ManufacturerId);

            Map(m => m.Name);

            References(m => m.Country)
                .Column("CountryId")
                .Not.Nullable()
                .Fetch.Join();
        }
    }
}
