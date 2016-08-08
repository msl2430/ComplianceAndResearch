namespace EngineCell.Application.ViewModels.TestDisplay
{
    public class ManualTimeConfigViewModel : BaseViewModel
    {
        private bool _isError { get; set; }
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; OnPropertyChanged("IsError"); }
        }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public bool IsCancel { get; set; }
        
    }
}
