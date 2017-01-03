namespace EngineCell.Application.ViewModels.Widget
{
    public class DynoPidDisplayViewModel : BaseViewModel
    {
        private decimal _setpoint { get; set; }
        public decimal Setpoint
        {
            get { return _setpoint; }
            set { _setpoint = value; OnPropertyChanged("Setpoint"); }
        }

        private bool _isActive { get; set; }
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); }
        }

        public string Name { get; set; }

        public DynoPidDisplayViewModel(string name, bool isActive = false)
        {
            Name = name;
            Setpoint = 0;
            IsActive = isActive;
        }
    }
}
