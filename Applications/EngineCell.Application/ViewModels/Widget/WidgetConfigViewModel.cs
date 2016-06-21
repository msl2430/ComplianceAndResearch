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

        public WidgetConfigViewModel(IApplicationSessionFactory appSession)
        {
            ApplicationSessionFactory = appSession;
            VentCtrl1 = new VentilationControlViewModel();
        }
    }
}
