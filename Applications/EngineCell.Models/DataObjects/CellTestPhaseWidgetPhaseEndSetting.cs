using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.CellTestPhaseWidget_PhaseEndSetting
    /// </summary>
    public class CellTestPhaseWidgetPhaseEndSetting
    {
        public virtual int CellTestPhaseWidgetPhaseEndSettingId { get; set; }
        public virtual int CellTestPhaseWidgetId { get; set; }
        public virtual WidgetConstants.PhaseEndSetting PhaseEndSettingId { get; set; }
        public virtual string Value { get; set; }
    }
}
