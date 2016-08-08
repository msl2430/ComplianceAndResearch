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

        private bool _isVisible { get; set; }
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; OnPropertyChanged("IsVisible"); }
        }

        public RemainingTimeViewModel()
        {
            RemainingTime = "00:00:00";
        }
    }
}
