using EngineCell.Core.Constants;

namespace EngineCell.Application.ViewModels.Pid
{
    public class PidDisplayViewModel : BaseViewModel
    {
        public ControlConstants.PidType PidType { get; set; }

        private PidDisplayModel _pidConfig { get; set; }
        public PidDisplayModel PidConfig { get { return _pidConfig; } set { _pidConfig = value; OnPropertyChanged("PidConfig"); } }
    }
}
