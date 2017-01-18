using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPhaseTriggerMap : ClassMap<CellTestPhaseTrigger>
    {
        public CellTestPhaseTriggerMap()
        {
            Table("CellTestPhaseTrigger");

            Id(t => t.CellTestPhaseTriggerId);

            Map(t => t.CellTestPhaseId);
            Map(t => t.CellPointId);
            Map(t => t.LowValue);
            Map(t => t.HighValue);
            Map(t => t.SecondsThreshold);
            Map(t => t.ResultTypeId).CustomType<WidgetConstants.TriggerResultType>();
            Map(t => t.ResultTypeParameter);
            Map(t => t.IsEmail);
        }
    }

    internal sealed class CellTestPhaseTriggerExtendedMap : ClassMap<CellTestPhaseTriggerExtended>
    {
        public CellTestPhaseTriggerExtendedMap()
        {
            Table("CellTestPhaseTrigger");

            Id(t => t.CellTestPhaseTriggerId);

            Map(t => t.CellTestPhaseId);
            Map(t => t.LowValue);
            Map(t => t.HighValue);
            Map(t => t.SecondsThreshold);
            Map(t => t.ResultTypeId).CustomType<WidgetConstants.TriggerResultType>();
            Map(t => t.ResultTypeParameter);
            Map(t => t.IsEmail);

            References(t => t.CellPoint)
                .Column("CellPointId")
                .Fetch.Join();
        }
    }
}
