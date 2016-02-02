namespace EngineCell.Application.ViewModels.Clocks
{
    public class RemainingTimeViewModel : BaseViewModel
    {
        private string _remainingTime { get; set; }

        public string RemainingTime
        {
            get { return _remainingTime; }
            set { _remainingTime = value; OnPropertyChanged("RemainingTime"); }
        }

        public RemainingTimeViewModel()
        {
            RemainingTime = "00:00:00";
        }
    }
}
