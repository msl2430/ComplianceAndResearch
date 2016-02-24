using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellMap : ClassMap<Cell>
    {
        public CellMap()
        {
            Table("Cell");
            Cache.NonStrictReadWrite();

            Id(c => c.CellId);

            Map(c => c.Name);
            Map(c => c.IpAddress);
            Map(c => c.PortNumber);
            Map(c => c.Description);
        }
    }
}
