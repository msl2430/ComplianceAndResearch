using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPhaseWidgetMap : ClassMap<CellTestPhaseWidget>
    {
        public CellTestPhaseWidgetMap()
        {
            Table("CellTestPhaseWidget");

            Id(t => t.CellTestPhaseWidgetId);

            Map(t => t.CellTestPhaseId);
            Map(t => t.WidgetId).CustomType<WidgetConstants.Widget>();
        }
    }

    internal sealed class CellTestPhaseWidgetExtendedMap : ClassMap<CellTestPhaseWidgetExtended>
    {
        public CellTestPhaseWidgetExtendedMap()
        {
            Table("CellTestPhaseWidget");

            Id(t => t.CellTestPhaseWidgetId);

            Map(t => t.CellTestPhaseId);
            Map(t => t.WidgetId).CustomType<WidgetConstants.Widget>();

            HasMany(t => t.Settings)
                .KeyColumn("CellTestPhaseWidgetId")
                .NotFound.Ignore()
                .Fetch.Subselect();            
        }
    }
}
