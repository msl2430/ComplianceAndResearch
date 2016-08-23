using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.WidgetSettingValue
    /// </summary>
    public class WidgetSettingValue
    {
        public virtual int WidgetSettingValueId { get; set; }
        public virtual WidgetConstants.Widget WidgetId { get; set; }
        public virtual int CellId { get; set; }
        public virtual WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public virtual string Value { get; set; }
    }
}
