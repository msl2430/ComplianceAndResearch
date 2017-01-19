using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class CellTestPhaseWidgetModel : BaseModel
    {
        public int CellTestPhaseWidgetId { get; set; }
        public int CellTestPhaseId { get; set; }
        public WidgetConstants.Widget WidgetId { get; set; }
        public IList<WidgetSettingModel> Settings { get; set; }

        public bool IsRunning { get; set; }
        public bool IsComplete { get; set; }
        public bool IsError { get; set; }
        public string ErrorReason { get; set; }

        public CellTestPhaseWidgetModel()
        {
            Settings = new List<WidgetSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidget obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Settings = new List<WidgetSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidgetExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Settings = obj.Settings.IsNotNullOrEmpty() ? obj.Settings.Select(s => new WidgetSettingModel(s)).ToList() : new List<WidgetSettingModel>();
        }

        public CellTestPhaseWidgetModel(CellTestPhaseWidget obj, IList<CellTestPhaseWidgetSetting> settings)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Settings = settings.IsNotNullOrEmpty() ? settings.Select(s => new WidgetSettingModel(s)).ToList() : new List<WidgetSettingModel>();
        }
    }
}
