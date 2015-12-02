using FluentNHibernate.Mapping;
using Robot.Core.Constants;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    public sealed class RoadTestChartMap : ClassMap<RoadTestChart>
    {
        public RoadTestChartMap()
        {
            Table("RoadTestChart");
            Cache.NonStrictReadWrite();

            Id(rtc => rtc.RoadTestChartId);

            Map(rtc => rtc.Name);
            Map(rtc => rtc.Status).CustomType<StatusConstants.SqlStatus>();
        }
    }

    public sealed class RoadTestChartExtendedMap : ClassMap<RoadTestChartExtended>
    {
        public RoadTestChartExtendedMap()
        {
            Table("RoadTestChart");
            Cache.NonStrictReadWrite();

            Id(rtc => rtc.RoadTestChartId);

            Map(rtc => rtc.Name);
            Map(rtc => rtc.Status).CustomType<StatusConstants.SqlStatus>();

            HasMany(rtc => rtc.RoadTestPoints)
                .KeyColumn("RoadTestChartId")
                .Not.LazyLoad()
                .Fetch.Subselect();
        }
    }
}
