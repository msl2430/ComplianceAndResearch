using System;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class WidgetSettingValueModel : BaseModel, IDisposable
    {
        public int WidgetSettingValueId { get; set; }
        public WidgetConstants.Widget WidgetId { get; set; }
        public int CellId { get; set; }
        public WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public string Value { get; set; }

        public WidgetSettingValueModel() { }

        public WidgetSettingValueModel(WidgetSettingValue obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }

        public void Dispose()
        {
            
        }
    }
}
