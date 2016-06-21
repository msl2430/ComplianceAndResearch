namespace EngineCell.Application.ViewModels.Widget
{
    public class VentilationControlDisplayViewModel : BaseViewModel
    {
        private decimal _inside { get; set; }
        public decimal Inside { get { return _inside; } set { _inside = value; OnPropertyChanged("Inside"); } }

        private decimal _outside { get; set; }

        public decimal Outside { get { return _outside; } set { _outside = value; OnPropertyChanged("Outside"); } }

        private decimal _output { get; set; }
        public decimal Output { get { return _output; } set { _output = value; OnPropertyChanged("Output"); } }

        private bool _isActive { get; set; }
        public bool IsActive { get { return _isActive; } set { _isActive = value; OnPropertyChanged("IsActive"); } }

        public string Name { get; set; }

        public VentilationControlDisplayViewModel(string name)
        {
            Name = name;
            IsActive = false;
            Inside = 0;
            Output = 0;
            Outside = 0;
        }
    }
}
