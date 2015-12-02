namespace Robot.Models.DataObjects
{
    /// <summary>
    /// dbo.GearRatio
    /// </summary>
    public class GearRatio
    {
        public virtual int GearRatioId { get; set; }
        public virtual int ModelId { get; set; }
        public virtual int Gear { get; set; }
        public virtual decimal Ratio { get; set; }
    }
}
