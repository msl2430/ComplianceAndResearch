using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    public class CellTestPhaseWidgetSetting
    {
        public virtual int CellTestPhaseWidgetSettingId { get; set; }
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual WidgetConstants.WidgetSetting WidgetSettingId { get; set; }
        public virtual string Value { get; set; }
    }
}
