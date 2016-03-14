using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellPointAlarmMap : ClassMap<CellPointAlarm>
    {
        public CellPointAlarmMap()
        {
            Table("CellPointAlarm");

            Id(pa => pa.CellPointAlarmId);

            Map(pa => pa.CellPointId);
            Map(pa => pa.LowShutdownWarning);
            Map(pa => pa.LowShutdownExecute);
            Map(pa => pa.HighShutdownWarning);
            Map(pa => pa.HighShutdownExecute);
            Map(pa => pa.UpdateDateTime);
        }
    }
}
