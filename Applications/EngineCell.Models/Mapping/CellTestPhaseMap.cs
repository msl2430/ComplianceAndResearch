using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPhaseMap : ClassMap<CellTestPhase>
    {
        public CellTestPhaseMap()
        {
            Table("CellTestPhase");

            Id(t => t.CellTestPhaseId);

            Map(t => t.CellTestId);
            Map(t => t.PhaseOrder);
            Map(t => t.Name);
        }
    }

    internal sealed class CellTestPhaseExtendedMap : ClassMap<CellTestPhaseExtended>
    {
        public CellTestPhaseExtendedMap()
        {
            Table("CellTestPhase");

            Id(t => t.CellTestPhaseId);

            Map(t => t.CellTestId);
            Map(t => t.PhaseOrder);
            Map(t => t.Name);

            HasMany<CellTestPhaseWidgetExtended>(t => t.Widgets)
                .KeyColumn("CellTestPhaseId")
                .NotFound.Ignore()
                .Fetch.Subselect();

            HasMany(t => t.Triggers)
                .KeyColumn("CellTestPhaseId")
                .NotFound.Ignore()
                .Fetch.Subselect();
        }
    }
}
