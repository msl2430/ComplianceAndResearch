using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Phase
{
    [Serializable]
    public class LoadPhaseViewModel : BaseViewModel
    {
        public IList<CellTestPhaseModel> Phases { get; set; }

        public CellTestPhaseModel PhaseToCopy { get; set; }
    }
}
