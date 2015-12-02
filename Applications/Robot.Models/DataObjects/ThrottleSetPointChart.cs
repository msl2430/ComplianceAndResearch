using Robot.Core.Constants;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Robot.Models.DataObjects
{
    public class ThrottleSetPointChart
    {
        public virtual int ThrottleSetPointChartId { get; set; }
        public virtual int ModelId { get; set; }
        public virtual string Name { get; set; }
        public virtual StatusConstants.SqlStatus Status { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
    }

    public class ThrottleSetPointChartExtended
    {
        public virtual int ThrottleSetPointChartId { get; set; }
        public virtual int ModelId { get; set; }
        public virtual string Name { get; set; }
        public virtual StatusConstants.SqlStatus Status { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
        public virtual IList<ThrottleSetPoint> SetPoints { get; set; }
    }
}
