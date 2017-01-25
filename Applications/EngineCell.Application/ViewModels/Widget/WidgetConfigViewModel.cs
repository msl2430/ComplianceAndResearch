using EngineCell.Application.Factories;

namespace EngineCell.Application.ViewModels.Widget
{
    public class WidgetConfigViewModel : BaseViewModel
    {
        private VentilationControlViewModel _ventCtrl1 { get; set; }
        public VentilationControlViewModel VentCtrl1 {
            get { return _ventCtrl1; }
            set { _ventCtrl1 = value; OnPropertyChanged("VentCtrl1"); }
        }

        private DynoPidConfigViewModel _dynoPid { get; set; }
        public DynoPidConfigViewModel DynoPid
        {
            get { return _dynoPid; }
            set { _dynoPid = value; OnPropertyChanged("DynoPid"); }
        }

      
        private bool _isDirty { get; set; }
        public bool IsDirty {
            get { return _isDirty; }
            set { _isDirty = value; OnPropertyChanged("IsDirty"); }
        }

        public WidgetConfigViewModel(IApplicationSessionFactory appSession)
        {
            ApplicationSessionFactory = appSession;
            VentCtrl1 = new VentilationControlViewModel();
            DynoPid = new DynoPidConfigViewModel();
            
        }
    }
}
