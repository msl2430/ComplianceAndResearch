using FluentNHibernate.Mapping;
using Robot.Core.Constants;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class ThrottleSetPointChartMap : ClassMap<ThrottleSetPointChart>
    {
        public ThrottleSetPointChartMap()
        {
            Table("ThrottleSetPointChart");

            Id(tspc => tspc.ThrottleSetPointChartId);

            Map(tspc => tspc.ModelId);
            Map(tspc => tspc.Name);
            Map(tspc => tspc.Status).CustomType<StatusConstants.SqlStatus>();
            Map(tspc => tspc.CreationDateTime);
        }
    }

    internal sealed class ThrottleSetPointCharExtendedtMap : ClassMap<ThrottleSetPointChartExtended>
    {
        public ThrottleSetPointCharExtendedtMap()
        {
            Table("ThrottleSetPointChart");

            Id(tspc => tspc.ThrottleSetPointChartId);

            Map(tspc => tspc.ModelId);
            Map(tspc => tspc.Name);
            Map(tspc => tspc.Status).CustomType<StatusConstants.SqlStatus>();
            Map(tspc => tspc.CreationDateTime);

            HasMany(tspc => tspc.SetPoints)
                .KeyColumn("ThrottleSetPointChartId")
                .Not.LazyLoad()
                .Fetch.Subselect();
        }
    }
}
