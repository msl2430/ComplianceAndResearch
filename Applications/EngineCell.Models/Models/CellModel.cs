using System;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class CellModel : BaseModel
    {
        public int CellId { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int PortNumber { get; set; }
        public string Description { get; set; }

        public CellModel() { }

        public CellModel(Cell obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
