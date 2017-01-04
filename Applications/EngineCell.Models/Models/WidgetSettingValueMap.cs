using System;
using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Models
{
    [Obsolete("Remove", false)]
    internal sealed class WidgetSettingValueMap : ClassMap<WidgetSettingValue>
    {
        public WidgetSettingValueMap()
        {
            Table("WidgetSettingValue");

            Id(w => w.WidgetSettingValueId);

            Map(w => w.WidgetId).CustomType<WidgetConstants.Widget>();
            Map(w => w.CellId);
            Map(w => w.WidgetSettingId).CustomType<WidgetConstants.WidgetSetting>();
            Map(w => w.Value);
        }
    }
}
