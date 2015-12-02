namespace Robot.Models.DataObjects
{
    public class ThrottleSetPoint
    {
        public virtual int ThrottleSetPointId {get; set; }
        public virtual int ThrottleSetPointChartId { get; set; }
        public virtual int Speed { get; set; }
        public virtual int Gear { get; set; }
        public virtual decimal Acceleration { get; set; }
        public virtual decimal SetPoint { get; set; }
    }
}
