using System.Windows;
using System.Threading.Tasks;
using Robot.Application.Factories;
using Robot.Application.Services.WorkerServices;
using Robot.Application.ViewModels;

namespace Robot.Application.Views.OptoMonitor
{
    /// <summary>
    /// Interaction logic for OptoMonitorView.xaml
    /// </summary>
    public partial class OptoMonitorView : BaseWindowView
    {
        private OptoMonitorViewModel OptoMonitorViewModel { get; set; }
        private IOptoMonitorWorkerService OptoMonitorWorkerService { get; set; }

        public OptoMonitorView(IApplicationSessionFactory applicationSessionFactory)
        {
            InitializeComponent();
            OptoMonitorViewModel = new OptoMonitorViewModel() { ApplicationSessionFactory = applicationSessionFactory};
        }

        private void OptoMonitorView_OnLoaded(object sender, RoutedEventArgs e)
        {
            OptoMonitorWorkerService = new OptoMonitorWorkerService(OptoMonitorViewModel);
            Task.Run(() => OptoMonitorWorkerService.DoWork()).ConfigureAwait(false);
            DataContext = OptoMonitorViewModel;
        }
    }
}
