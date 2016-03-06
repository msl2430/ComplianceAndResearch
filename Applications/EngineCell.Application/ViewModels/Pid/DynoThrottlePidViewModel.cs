using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCell.Application.ViewModels.Pid
{
    public class DynoThrottlePidViewModel : BaseViewModel
    {
        private PidDisplayModel _dyno { get; set; }
        public PidDisplayModel Dyno
        {
            get { return _dyno; }
            set { _dyno = value; OnPropertyChanged("Dyno"); }
        }

        private PidDisplayModel _throttle { get; set; }
        public PidDisplayModel Throttle
        {
            get { return _throttle; }
            set { _throttle = value;  OnPropertyChanged("Throttle"); }
        }


    }
}
