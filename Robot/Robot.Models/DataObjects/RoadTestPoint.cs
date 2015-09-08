namespace Robot.Models.DataObjects
{
    /// <summary>
    /// dbo.RoadTestPoint
    /// </summary>
    public class RoadTestPoint
    {
        public virtual int RoadTestPointId { get; set; }
        public virtual int RoadTestChartId { get; set; }
        public virtual decimal SecondFromStart { get; set; }
        public virtual decimal Speed { get; set; }
    }
}
