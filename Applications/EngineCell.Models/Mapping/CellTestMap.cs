using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestMap : ClassMap<CellTest>
    {
        public CellTestMap()
        {
            Table("CellTest");

            Id(t => t.CellTestId);

            Map(t => t.CellId);
            Map(t => t.Name);
            Map(t => t.Description);
            Map(t => t.StartTime);
            Map(t => t.EndTime);
        }
    }

    internal sealed class CellTestExtendedMap : ClassMap<CellTestExtended>
    {
        public CellTestExtendedMap()
        {
            Table("CellTest");

            Id(t => t.CellTestId);

            Map(t => t.CellId);
            Map(t => t.Name);
            Map(t => t.Description);
            Map(t => t.StartTime);
            Map(t => t.EndTime);

            HasMany(t => t.Phases)
                .KeyColumn("CellTestId")
                .NotFound.Ignore()
                .Fetch.Subselect();
        }
    }
}
