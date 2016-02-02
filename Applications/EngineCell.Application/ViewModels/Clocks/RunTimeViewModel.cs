namespace EngineCell.Application.ViewModels.Clocks
{
    public class RunTimeViewModel : BaseViewModel
    {
        private string _runTime { get; set; }

        public string RunTime
        {
            get { return _runTime; }
            set { _runTime = value; OnPropertyChanged("RunTime"); }
        }

        public RunTimeViewModel()
        {
            RunTime = "00:00:00";
        }
    }
}
