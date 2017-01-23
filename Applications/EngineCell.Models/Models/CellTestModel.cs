using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    public class CellTestModel : BaseModel
    {
        public int CellTestId { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual DateTime CreationDateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public IList<CellTestPhaseModel> Phases { get; set; }

        public bool IsRunning { get; set; }
        public string CreationDateString => CreationDateTime.ToString("MM/dd/yyyy hh:mmtt");

        public CellTestModel () {  }

        public CellTestModel(CellTest obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Phases = new List<CellTestPhaseModel>();
        }

        public CellTestModel(CellTestExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            CellId = obj.Cell.CellId;
            CellName = obj.Cell.Name;
            Phases = obj.Phases.IsNotNullOrEmpty() ? obj.Phases.Select(p => new CellTestPhaseModel(p)).ToList() : new List<CellTestPhaseModel>();
        }

        public CellTestModel(CellTest obj, IList<CellTestPhaseExtended> phases)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Phases = phases.IsNotNullOrEmpty() ? phases.Select(p => new CellTestPhaseModel(p)).ToList() : new List<CellTestPhaseModel>();
        }
    }
}
