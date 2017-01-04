using System.Collections.Generic;
using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    public class CellTestPhaseWidget
    {
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual int WidgetId { get; set; }
        public virtual WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public virtual string Value { get; set; }
    }

    public class CellTestPhaseWidgetExtended
    {
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual int WidgetId { get; set; }
        public virtual WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public virtual string Value { get; set; }
        public virtual IList<CellTestPhaseWidgetPhaseEndSetting> PhaseEndSettings { get; set; }
    }
}
