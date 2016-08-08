using EngineCell.Core.Constants;
using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestMap : ClassMap<CellTest>
    {
        public CellTestMap()
        {
            Table("CellTest");

            Id(t => t.CellTestId);

            Map(t => t.CellId);
            Map(t => t.CellTestTypeId).CustomType<ControlConstants.CellTestType>();
            Map(t => t.StartTime);
            Map(t => t.EndTime);
        }
    }
}
