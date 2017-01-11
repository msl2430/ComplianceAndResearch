using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.TestDisplay
{
    public class TestDisplayViewModel : BaseViewModel
    {
        private bool _isTestRunning { get; set; }
        public bool IsTestRunning { get { return _isTestRunning; } set { _isTestRunning = value; OnPropertyChanged("IsTestRunning"); } }

        private StripChartViewModel _chartViewModel { get; set; }
        public StripChartViewModel ChartViewModel { get { return _chartViewModel; } set { _chartViewModel = value; OnPropertyChanged("ChartViewModel"); } }

        private ObservableCollection<PointDataModel> _visiblePoints { get; set; }
        public ObservableCollection<PointDataModel> VisiblePoints {
            get { return _visiblePoints; }
            set { _visiblePoints = value; OnPropertyChanged("VisiblePoints"); }
        }

        private ObservableCollection<PointDataModel> _leftVisiblePoints { get; set; }
        public ObservableCollection<PointDataModel> LeftVisiblePoints
        {
            get { return _leftVisiblePoints; }
            set { _leftVisiblePoints = value; OnPropertyChanged("LeftVisiblePoints"); }
        }

        private ObservableCollection<PointDataModel> _rightVisiblePoints { get; set; }
        public ObservableCollection<PointDataModel> RightVisiblePoints
        {
            get { return _rightVisiblePoints; }
            set { _rightVisiblePoints = value; OnPropertyChanged("RightVisiblePoints"); }
        }

        private bool _isManualTest { get; set; }
        public bool IsManualTest {
            get { return _isManualTest; }
            set { _isManualTest = value; OnPropertyChanged("IsManualTest"); }
        }

        private ObservableCollection<PhaseViewModel> _phases { get; set; }
        public ObservableCollection<PhaseViewModel> Phases
        {
            get { return _phases;}
            set { _phases = value; OnPropertyChanged("Phases"); }
        }

        public IPointWorkerService PointWorkerService { get; set; }

        public TestDisplayViewModel(IApplicationSessionFactory appSession, StripChartViewModel chartViewModel)
        {
            ZIndex = 1;
            ApplicationSessionFactory = appSession;
            IsManualTest = false;
            ChartViewModel = chartViewModel;            
            VisiblePoints = new ObservableCollection<PointDataModel>();
            LeftVisiblePoints = new ObservableCollection<PointDataModel>();
            RightVisiblePoints = new ObservableCollection<PointDataModel>();
            Phases = new ObservableCollection<PhaseViewModel>();

            UpdateViewModel();
        }

        private void UpdateViewModel()
        {
            if (ApplicationSessionFactory.CurrentCell == null || ApplicationSessionFactory.CurrentCellTest == null || ApplicationSessionFactory.CurrentCellTest.Phases.IsNullOrEmpty())
                return;

            UpdatePhases(ApplicationSessionFactory.CurrentCellTest.Phases);
            UpdateVisibleCellPoints();
        }

        public void UpdatePhases(IList<CellTestPhaseModel> phases)
        {
            Phases.Clear();
            var tempPhases = new ObservableCollection<PhaseViewModel>();
            foreach (var phase in phases)
            {
                tempPhases.Add(new PhaseViewModel(phase));
            }
            Phases = tempPhases;
        }

        public void UpdateVisibleCellPoints()
        {
            VisiblePoints.Clear();
            if (ApplicationSessionFactory.CurrentCell == null || ApplicationSessionFactory.CurrentCellTest == null || ApplicationSessionFactory.CurrentCellTest.Phases.IsNullOrEmpty())
                return;
            foreach (var point in ApplicationSessionFactory.CellPoints.Where(cp => cp.IsAverage))
            {
                VisiblePoints.Add(point);
            }

            LeftVisiblePoints.Clear();
            RightVisiblePoints.Clear();

            for (var i = 0; i < VisiblePoints.Count; i++)
            {
                if (i%2 == 0)
                    LeftVisiblePoints.Add(VisiblePoints[i]);
                else
                    RightVisiblePoints.Add(VisiblePoints[i]);
            }
        }

        public void CellSectionChange()
        {
            UpdateViewModel();
            ChartViewModel.CellSelectionChange();
        }
    }
}
