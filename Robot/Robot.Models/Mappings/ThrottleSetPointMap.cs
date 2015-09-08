using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class ThrottleSetPointMap : ClassMap<ThrottleSetPoint>
    {
        public ThrottleSetPointMap()
        {
            Table("ThrottleSetPoint");

            Id(tsp => tsp.ThrottleSetPointId);

            Map(tsp => tsp.ThrottleSetPointChartId);
            Map(tsp => tsp.Speed);
            Map(tsp => tsp.Gear);
            Map(tsp => tsp.Acceleration);
            Map(tsp => tsp.SetPoint);
        }
    }
}
