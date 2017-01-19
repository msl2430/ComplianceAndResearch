using System.Collections.Generic;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.Models;
using Remotion.Linq.Collections;

namespace EngineCell.Application.ViewModels.Phase
{
    public class TriggerConfigViewModel : BaseViewModel
    {
        private bool _isAddTrigger { get; set; }
        public bool IsAddTrigger
        {
            get { return _isAddTrigger; }
            set { _isAddTrigger = value; OnPropertyChanged("IsAddTrigger"); }
        }

        private CellTestPhaseModel _phase { get; set; }
        public CellTestPhaseModel Phase
        {
            get { return _phase; }
            set { _phase = value; OnPropertyChanged("Phase"); }
        }
        
        public IList<IdNamePair> PhaseNames { get; set; }

        public TriggerConfigViewModel(IApplicationSessionFactory appSession, CellTestPhaseModel phase, IList<IdNamePair> phaseNames )
        {
            ApplicationSessionFactory = appSession;
            Phase = phase;
            PhaseNames = phaseNames;
        }
    }
}
