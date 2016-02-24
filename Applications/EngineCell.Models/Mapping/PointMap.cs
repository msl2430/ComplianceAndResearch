using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class PointMap : ClassMap<Point>
    {
        public PointMap()
        {
            Table("Point");
            Cache.NonStrictReadWrite();

            Id(p => p.PointId);

            Map(p => p.Name);
            Map(p => p.IsInput);
            Map(p => p.IsAnalog);
            Map(p => p.PointGroupId).CustomType<PointGroup>();
        }
    }
}
