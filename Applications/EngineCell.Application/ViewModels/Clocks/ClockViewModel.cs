namespace EngineCell.Application.ViewModels.Clocks
{
    public class ClockViewModel : BaseViewModel
    {
        private string _timeOfDay { get; set; }

        public string TimeOfDay
        {
            get { return _timeOfDay; }
            set { _timeOfDay = value; OnPropertyChanged("TimeOfDay"); }
        }
    }
}
