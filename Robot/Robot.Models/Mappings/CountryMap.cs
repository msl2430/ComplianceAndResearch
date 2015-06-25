using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Table("Country");
            Cache.NonStrictReadWrite();

            Id(c => c.CountryId);

            Map(c => c.Name);
            Map(c => c.Abbreviation);
        }
    }
}
