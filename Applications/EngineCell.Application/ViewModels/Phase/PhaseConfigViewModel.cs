using EngineCell.Application.Factories;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using Remotion.Linq.Collections;

namespace EngineCell.Application.ViewModels.Phase
{
    public class PhaseConfigViewModel : BaseViewModel
    {
        private ObservableCollection<CellTestPhaseModel> _phases { get; set; }
        public ObservableCollection<CellTestPhaseModel> Phases
        {
            get { return _phases;}
            set { _phases = value; OnPropertyChanged("Phases"); }
        }

        public PhaseConfigViewModel(IApplicationSessionFactory appSession)
        {
            ApplicationSessionFactory = appSession;
            Phases = new ObservableCollection<CellTestPhaseModel>();
            UpdatePhases();
        }

        private void UpdatePhases()
        {
            if (ApplicationSessionFactory.CurrentCell == null || ApplicationSessionFactory.CurrentCellTest == null || ApplicationSessionFactory.CurrentCellTest.Phases.IsNullOrEmpty())
                return;

            var tempPhases = new ObservableCollection<CellTestPhaseModel>();
            foreach (var phase in ApplicationSessionFactory.CurrentCellTest.Phases)
            {
                tempPhases.Add(phase);
            }

            Phases = tempPhases;
        }

        public void CellSelectionChange()
        {
            UpdatePhases();
        }
    }
}
