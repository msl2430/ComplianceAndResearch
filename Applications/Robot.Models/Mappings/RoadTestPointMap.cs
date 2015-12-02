using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    public sealed class RoadTestPointMap : ClassMap<RoadTestPoint>
    {
        public RoadTestPointMap()
        {
            Table("RoadTestPoint");
            Cache.NonStrictReadWrite();

            Id(rtp => rtp.RoadTestPointId);

            Map(rtp => rtp.RoadTestChartId);
            Map(rtp => rtp.SecondFromStart);
            Map(rtp => rtp.Speed);
        }
    }
}
