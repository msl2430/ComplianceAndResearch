namespace EngineCell.Models.DataObjects
{
    /// <summary>
    /// dbo.Cell
    /// </summary>
    public class Cell
    {
        public virtual int CellId { get; set; }
        public virtual string Name { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual int PortNumber { get; set; }
        public virtual string Description { get; set; }
    }
}
