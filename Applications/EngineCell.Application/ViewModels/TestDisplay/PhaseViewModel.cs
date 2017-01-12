using System;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.TestDisplay
{
    [Serializable]
    public class PhaseViewModel : BaseViewModel
    {
        public int CellTestPhaseId { get; set; }
        public int CellTestId { get; set; }
        public int PhaseOrder { get; set; }
        public string Name { get; set; }

        private DateTime? _startDateTime { get; set; }
        public DateTime? StartDateTime { get { return _startDateTime; } set { _startDateTime = value; OnPropertyChanged("StartDateTime"); OnPropertyChanged("IsWaiting"); OnPropertyChanged("IsCompleted"); OnPropertyChanged("StartDateTimeString"); } }

        private DateTime? _endDateTime { get; set; }
        public DateTime? EndDateTime { get { return _endDateTime; } set { _endDateTime = value; OnPropertyChanged("EndDateTime"); OnPropertyChanged("IsWaiting"); OnPropertyChanged("IsCompleted"); OnPropertyChanged("EndDateTimeString"); } }

        public string StartDateTimeString { get { return StartDateTime == null ? "N/A" : Convert.ToDateTime(StartDateTime).ToString("MM/dd/yyy hh:mm tt"); } }
        public string EndDateTimeString { get { return EndDateTime == null ? "N/A" : Convert.ToDateTime(EndDateTime).ToString("MM/dd/yyy hh:mm tt"); } }

        private bool _isRunning { get; set; }
        public bool IsRunning { get { return _isRunning; } set { _isRunning = value; OnPropertyChanged("IsRunning"); OnPropertyChanged("IsWaiting"); OnPropertyChanged("IsCompleted"); } }
        public bool IsCompleted { get { return !IsRunning && EndDateTime != null; } }

        public PhaseViewModel() { }

        public PhaseViewModel(CellTestPhaseModel phase)
        {
            if (phase == null)
                return;

            CellTestPhaseId = phase.CellTestPhaseId;
            CellTestId = phase.CellTestId;
            PhaseOrder = phase.PhaseOrder;
            Name = phase.Name;
            IsRunning = false;
        }
    }
}
