using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
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

        private ObservableCollection<CellTestPhaseTriggerModel> _triggers { get; set; }
        public ObservableCollection<CellTestPhaseTriggerModel> Triggers
        {
            get { return _triggers; }
            set { _triggers = value; OnPropertyChanged("Triggers"); }
        }

        //public int CellTestPhaseId { get; set; }
        
        //private int _cellPointId { get; set; }
        //public int CellPointId
        //{
        //    get { return _cellPointId;}
        //    set { _cellPointId = value; OnPropertyChanged("CellPointId"); }
        //}

        //private string _cellPointName { get; set; }
        //public string CellPointName
        //{
        //    get { return _cellPointName; }
        //    set { _cellPointName = value; OnPropertyChanged("CellPointName"); }
        //}

        //private decimal _lowValue { get; set; }
        //public decimal LowValue
        //{
        //    get { return _lowValue; }
        //    set { _lowValue = value; OnPropertyChanged("LowValue"); }
        //}

        //private decimal _highValue { get; set; }
        //public decimal HighValue
        //{
        //    get { return _highValue; }
        //    set { _highValue = value; OnPropertyChanged("HighValue"); }
        //}

        //private decimal _secondsThreshold { get; set; }
        //public decimal SecondsThreshold
        //{
        //    get { return _secondsThreshold; }
        //    set { _secondsThreshold = value; OnPropertyChanged("SecondsThreshold"); }
        //}

        //private WidgetConstants.TriggerResultType _resultTypeId { get; set; }
        //public WidgetConstants.TriggerResultType ResultTypeId
        //{
        //    get { return _resultTypeId; }
        //    set { _resultTypeId = value; OnPropertyChanged("ResultTypeId"); }
        //}

        //private string _resultTypeParameter { get; set; }
        //public string ResulTypeParameter
        //{
        //    get { return _resultTypeParameter; }
        //    set { _resultTypeParameter = value; OnPropertyChanged("ResulTypeParameter"); }
        //}

        //private bool _isAlarm { get; set; }
        //public bool IsAlarm
        //{
        //    get { return _isAlarm; }
        //    set { _isAlarm = value; OnPropertyChanged("IsAlarm"); }
        //}

        //private bool _isEmail { get; set; }
        //public bool IsEmail
        //{
        //    get { return _isEmail; }
        //    set { _isEmail = value; OnPropertyChanged("IsEmail"); }
        //}

        public TriggerConfigViewModel(IApplicationSessionFactory appSession, CellTestPhaseModel phase)
        {
            ApplicationSessionFactory = appSession;
            Phase = phase;
            Triggers = new ObservableCollection<CellTestPhaseTriggerModel>();
            foreach (var trigger in Phase.Triggers)
            {
                Triggers.Add(trigger);
            }
        }
    }
}
