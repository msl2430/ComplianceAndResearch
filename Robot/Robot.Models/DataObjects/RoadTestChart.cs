using System.Collections.Generic;
using Robot.Core.Constants;

namespace Robot.Models.DataObjects
{
    /// <summary>
    /// dbo.RoadTestChart
    /// </summary>
    public class RoadTestChart
    {
        public virtual int RoadTestChartId { get; set; }
        public virtual string Name { get; set; }
        public virtual StatusConstants.SqlStatus Status { get; set; }
    }

    /// <summary>
    /// dbo.RoadTestChart
    /// </summary>
    public class RoadTestChartExtended
    {
        public virtual int RoadTestChartId { get; set; }
        public virtual string Name { get; set; }
        public virtual StatusConstants.SqlStatus Status { get; set; }
        public virtual IList<RoadTestPoint> RoadTestPoints { get; set; } 
    }
}
