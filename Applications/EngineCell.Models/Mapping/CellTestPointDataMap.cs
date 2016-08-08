using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellTestPointDataMap : ClassMap<CellTestPointData>
    {
        public CellTestPointDataMap()
        {
            Table("CellTestPointData");

            Id(ctpd => ctpd.CellTestPointDataId);

            Map(ctpd => ctpd.CellTestId);
            Map(ctpd => ctpd.CellPointId);
            Map(ctpd => ctpd.Data);
            Map(ctpd => ctpd.CaptureTime);
        }
    }

    internal sealed class CellTestPointDataExtendedMap : ClassMap<CellTestPointDataExtended>
    {
        public CellTestPointDataExtendedMap()
        {
            Table("CellTestPointData");

            Id(ctpd => ctpd.CellTestPointDataId);

            Map(ctpd => ctpd.Data);
            Map(ctpd => ctpd.CaptureTime);

            References(ctpd => ctpd.CellPoint)
                .Column("CellPointId")
                .Fetch.Join();

            References(ctpd => ctpd.CellTest)
                .Column("CellTestId")
                .Fetch.Join();
        }
    }
}
