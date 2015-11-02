using FluentNHibernate.Mapping;
using Robot.Models.DataObjects;

namespace Robot.Models.Mappings
{
    internal sealed class GearRatioMap : ClassMap<GearRatio>
    {
        public GearRatioMap()
        {
            Table("GearRatio");

            Id(gr => gr.GearRatioId);

            Map(gr => gr.ModelId);
            Map(gr => gr.Gear);
            Map(gr => gr.Ratio);
        }
    }
}
