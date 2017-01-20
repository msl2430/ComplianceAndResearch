using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
using EngineCell.Models.DataObjects;

namespace EngineCell.Models.Models
{
    [Serializable]
    public class CellTestPhaseModel : BaseModel
    {
        public int CellTestPhaseId { get; set; }
        public int CellTestId { get; set; }
        public int PhaseOrder { get; set; }
        public string Name { get; set; }
        public IList<CellTestPhaseWidgetModel> Widgets { get; set; }
        public IList<CellTestPhaseTriggerModel> Triggers { get; set; }

        public CellTestPhaseModel()
        {
            Widgets = new List<CellTestPhaseWidgetModel>();
            Triggers = new List<CellTestPhaseTriggerModel>();
        }

        public CellTestPhaseModel(CellTestPhase obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Widgets = new List<CellTestPhaseWidgetModel>();
            Triggers = new List<CellTestPhaseTriggerModel>();
        }

        public CellTestPhaseModel(CellTestPhaseExtended obj)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Widgets = obj.Widgets.IsNotNullOrEmpty() ? obj.Widgets.Select(w => new CellTestPhaseWidgetModel(w)).ToList() : new List<CellTestPhaseWidgetModel>();
            Triggers = obj.Triggers.IsNotNullOrEmpty() ? obj.Triggers.Select(t => new CellTestPhaseTriggerModel(t)).ToList() : new List<CellTestPhaseTriggerModel>();
        }

        public CellTestPhaseModel(CellTestPhase obj, IList<CellTestPhaseWidget> widgets)
        {
            if (obj == null)
                return;

            InstantiateFromDataObject(obj);
            Widgets = widgets.IsNotNullOrEmpty() ? widgets.Select(w => new CellTestPhaseWidgetModel(w)).ToList() : new List<CellTestPhaseWidgetModel>();
            Triggers = new List<CellTestPhaseTriggerModel>();
        }
    }
}
