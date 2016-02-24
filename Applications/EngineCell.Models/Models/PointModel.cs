using System;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class PointModel : BaseModel
    {
        public int PointId { get; set; }
        public string Name { get; set; }
        public bool IsInput { get; set; }
        public bool IsAnalog { get; set; }
        public PointGroup PoingGroupId { get; set; }

        public PointModel() { }

        public PointModel(Point obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
