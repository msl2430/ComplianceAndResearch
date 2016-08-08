using System;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    public class CellTestModel : BaseModel
    {
        public int CellTestId { get; set; }
        public int CellId { get; set; }
        public ControlConstants.CellTestType CellTestTypeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public CellTestModel () {  }

        public CellTestModel(CellTest obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
        }
    }
}
