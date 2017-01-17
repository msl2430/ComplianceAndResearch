using System.Collections.Generic;
using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    public class CellTestPhaseWidget
    {
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual WidgetConstants.Widget WidgetId { get; set; }
    }

    public class CellTestPhaseWidgetExtended
    {
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual int CellTestPhaseId { get; set; }
        public virtual WidgetConstants.Widget WidgetId { get; set; }
        public virtual IList<CellTestPhaseWidgetSetting> Settings { get; set; }
    }
}
