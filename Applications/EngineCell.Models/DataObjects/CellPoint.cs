namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.Cell_Point
    /// </summary>
    public class CellPoint
    {
        public virtual int CellPointId { get; set; }
        public virtual int CellId { get; set; }
        public virtual int PointId { get; set; }
        public virtual string CustomName { get; set; }
        public virtual bool IsRecord { get; set; }
        public virtual bool IsAverage { get; set; }
        public virtual int? AverageSeconds { get; set; }
        public virtual bool IncludeInStripChart { get; set; }
        public virtual decimal? StripChartScale { get; set; }
        public virtual bool IsActive { get; set; }
        //public virtual bool IsCustomValue { get; set; }
        //public virtual decimal? CustomValue { get; set; }
    }

    /// <summary>
    /// dbo.Cell_Point
    /// </summary>
    public class CellPointExtended
    {
        public virtual int CellPointId { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual Point Point { get; set; }
        public virtual string CustomName { get; set; }
        public virtual bool IsRecord { get; set; }
        public virtual bool IsAverage { get; set; }
        public virtual int? AverageSeconds { get; set; }
        public virtual bool IncludeInStripChart { get; set; }
        public virtual decimal? StripChartScale { get; set; }
        public virtual bool IsActive { get; set; }
        //public virtual bool IsCustomValue { get; set; }
        //public virtual decimal? CustomValue { get; set; }
    }
}
