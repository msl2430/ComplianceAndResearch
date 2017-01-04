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
            Map(t => t.WidgetId);
            Map(t => t.WidgetSettingId).CustomType<WidgetConstants.WidgetSetting>();
            Map(t => t.Value);
        }
    }

    internal sealed class CellTestPhaseWidgetExtendedMap : ClassMap<CellTestPhaseWidgetExtended>
    {
        public CellTestPhaseWidgetExtendedMap()
        {
            Table("CellTestPhaseWidget");

            Id(t => t.CellTestPhaseWidgetId);

            Map(t => t.CellTestPhaseId);
            Map(t => t.WidgetId);
            Map(t => t.WidgetSettingId).CustomType<WidgetConstants.WidgetSetting>();
            Map(t => t.Value);

            HasMany(t => t.PhaseEndSettings)
                .KeyColumn("CellTestPhaseWidgetId")
                .NotFound.Ignore()
                .Fetch.Join();
        }
    }
}
