using EngineCell.Application.Factories;
using EngineCell.Core.Constants;

namespace EngineCell.Application.ViewModels.Pid
{
    public class PidConfigViewModel : BaseViewModel
    {
        public ControlConstants.PidType PidType { get; set; }

        private decimal _input { get; set; }
        public decimal Input {
            get { return _input; }
            set { _input = value; OnPropertyChanged("Input"); }
        }

        private decimal _lowRange { get; set; }
        public decimal LowRange
        {
            get { return _lowRange; }
            set { _lowRange = value; OnPropertyChanged("LowRange"); }
        }

        private decimal _highRange { get; set; }
        public decimal HighRange
        {
            get { return _highRange; }
            set { _highRange = value; OnPropertyChanged("HighRange"); }
        }

        private decimal _output { get; set; }
        public decimal Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged("Output"); }
        }

        private decimal _lowerClamp { get; set; }
        public decimal LowerClamp
        {
            get { return _lowerClamp; }
            set { _lowerClamp = value; OnPropertyChanged("LowerClamp"); }
        }

        private decimal _upperClamp { get; set; }
        public decimal UpperClamp
        {
            get { return _upperClamp; }
            set { _upperClamp = value; OnPropertyChanged("UpperClamp"); }
        }

        private decimal _minChange { get; set; }
        public decimal MinChange
        {
            get { return _minChange; }
            set { _minChange = value; OnPropertyChanged("MinChange"); }
        }

        private decimal _maxChange { get; set; }
        public decimal MaxChange
        {
            get { return _maxChange; }
            set { _maxChange = value; OnPropertyChanged("MaxChange"); }
        }

        private decimal _setPoint { get; set; }
        public decimal SetPoint
        {
            get { return _setPoint; }
            set { _setPoint = value; OnPropertyChanged("SetPoint"); }
        }

        private decimal _gain { get; set; }
        public decimal Gain
        {
            get { return _gain; }
            set { _gain = value; OnPropertyChanged("Gain"); }
        }

        private decimal _tuneI { get; set; }
        public decimal TuneI
        {
            get { return _tuneI; }
            set { _tuneI = value; OnPropertyChanged("TuneI"); }
        }

        private decimal _tuneD { get; set; }
        public decimal TuneD
        {
            get { return _tuneD; }
            set { _tuneD = value; OnPropertyChanged("TuneD"); }
        }

        private decimal _feedFwdInitial { get; set; }
        public decimal FeedFwdInitial
        {
            get { return _feedFwdInitial; }
            set { _feedFwdInitial = value; OnPropertyChanged("FeedFwdInitial"); }
        }

        private decimal _feedFwdGain { get; set; }
        public decimal FeedFwdGain
        {
            get { return _feedFwdGain; }
            set { _feedFwdGain = value; OnPropertyChanged("FeedFwdGain"); }
        }

        public PidConfigViewModel(IApplicationSessionFactory sessionFactory)
        {
            ApplicationSessionFactory = sessionFactory;
        }
    }
}
