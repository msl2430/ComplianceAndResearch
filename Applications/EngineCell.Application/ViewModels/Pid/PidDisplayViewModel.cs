namespace EngineCell.Application.ViewModels.Pid
{
    public class PidDisplayViewModel : BaseViewModel
    {
        private PidDisplayModel _pidConfig { get; set; }
        public PidDisplayModel PidConfig { get { return _pidConfig; } set { _pidConfig = value; OnPropertyChanged("PidConfig"); } }
    }
}
