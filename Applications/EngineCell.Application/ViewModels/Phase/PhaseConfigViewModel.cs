using EngineCell.Application.Factories;
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
            foreach (var phase in appSession.CurrentCellTest.Phases)
            {
                Phases.Add(phase);
            }
        }
    }
}
