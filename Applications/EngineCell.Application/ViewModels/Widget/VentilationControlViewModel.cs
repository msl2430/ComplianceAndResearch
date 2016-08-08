using EngineCell.Core.Constants;

namespace EngineCell.Application.ViewModels.Widget
{
    public class VentilationControlViewModel : BaseViewModel
    {
        private ThermoCouple _inside { get; set; }
        public ThermoCouple Inside
        {
            get { return _inside; }
            set { _inside = value; OnPropertyChanged("Inside"); }
        }

        private ThermoCouple _outside { get; set; }
        public ThermoCouple Outside
        {
            get { return _outside; }
            set { _outside = value; OnPropertyChanged("Outside"); }
        }

        private AnalogOutput _output { get; set; }
        public AnalogOutput Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged("Output"); }
        }

        private decimal _gain { get; set; }
        public decimal Gain
        {
            get { return _gain; }
            set { _gain = value; OnPropertyChanged("Gain"); }
        }

        private decimal _setPoint { get; set; }
        public decimal SetPoint
        {
            get { return _setPoint; }
            set { _setPoint = value; OnPropertyChanged("SetPoint"); }
        }

        private bool _isActive { get;set; }
        public bool IsActive { get { return _isActive;} set { _isActive = value; OnPropertyChanged("IsActive"); } }

        public VentilationControlViewModel()
        {
            Inside = ThermoCouple.ThermoCouple1;
            Outside = ThermoCouple.ThermoCouple2;
            Output = AnalogOutput.Ao10Bipolar;
            IsActive = false;
        }
    }
}
