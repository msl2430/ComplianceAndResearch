using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPhaseWidgetPhaseEndSettingMap : ClassMap<CellTestPhaseWidgetPhaseEndSetting>
    {
        public CellTestPhaseWidgetPhaseEndSettingMap()
        {
            Table("CellTestPhaseWidget_PhaseEndSetting");

            Id(t => t.CellTestPhaseWidgetPhaseEndSettingId);

            Map(t => t.CellTestPhaseWidgetId);
            Map(t => t.PhaseEndSettingId).CustomType<WidgetConstants.PhaseEndSetting>();
            Map(t => t.Value);
        }
    }
}
