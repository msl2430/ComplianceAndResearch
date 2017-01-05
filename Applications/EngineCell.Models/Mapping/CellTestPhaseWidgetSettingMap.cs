using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPhaseWidgetSettingMap : ClassMap<CellTestPhaseWidgetSetting>
    {
        public CellTestPhaseWidgetSettingMap()
        {
            Table("CellTestPhaseWidget_Setting");

            Id(t => t.CellTestPhaseWidgetSettingId);

            Map(t => t.CellTestPhaseWidgetId);
            Map(t => t.WidgetSettingId).CustomType<WidgetConstants.WidgetSetting>();
            Map(t => t.Value);
        }
    }
}
