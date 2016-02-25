﻿using EngineCell.Models.DataObjects;
using FluentNHibernate.Mapping;

namespace EngineCell.Models.Mapping
{
    internal sealed class CellPointMap : ClassMap<CellPoint>
    {
        public CellPointMap()
        {
            Table("Cell_Point");
            Cache.NonStrictReadWrite();

            Id(cp => cp.CellPointId);

            Map(cp => cp.CellId);
            Map(cp => cp.PointId);
            Map(cp => cp.CustomName);
            Map(cp => cp.IncludeInStripChart);
            Map(cp => cp.StripChartScale);
        }
    }

    internal sealed class CellPointExtendedMap : ClassMap<CellPointExtended>
    {
        public CellPointExtendedMap()
        {
            Table("Cell_Point");
            Cache.NonStrictReadWrite();

            Id(cp => cp.CellPointId);

            Map(cp => cp.CustomName);
            Map(cp => cp.IncludeInStripChart);
            Map(cp => cp.StripChartScale);

            References(cp => cp.Cell)
                .Column("CellId")
                .Fetch.Join();

            References(cp => cp.Point)
                .Column("PointId")
                .Fetch.Join();
        }
    }
}