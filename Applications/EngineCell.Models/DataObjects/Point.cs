using EngineCell.Core.Constants;

namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.Public
    /// </summary>
    public class Point
    {
        public virtual int PointId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsInput { get; set; }
        public virtual bool IsAnalog { get; set; }
        public virtual PointGroup PointGroupId { get; set; }
    }
}
