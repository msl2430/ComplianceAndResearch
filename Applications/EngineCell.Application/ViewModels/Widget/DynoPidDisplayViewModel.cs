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

        public string Name { get; set; }

        public DynoPidDisplayViewModel(string name, bool isActive = false)
        {
            Name = name;
            Setpoint = 0;
        }
    }
}
