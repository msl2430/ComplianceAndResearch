using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Phase
{
    /// <summary>
    /// Interaction logic for LoadPhaseDialog.xaml
    /// </summary>
    public partial class LoadPhaseDialog : Window
    {
        public LoadPhaseViewModel ViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public LoadPhaseDialog(IApplicationSessionFactory appSession)
        {
            InitializeComponent();
            ViewModel = new LoadPhaseViewModel() {ApplicationSessionFactory = appSession};
        }

        private void LoadPhaseDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Phases = WidgetRepository.GetCellPhases(ViewModel.ApplicationSessionFactory.CurrentCellTest.CellId).OrderBy(p => p.CellTestName).ThenBy(p => p.PhaseOrder).ToList();
            DataContext = ViewModel;
        }

        private void HistoryList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var phase = (CellTestPhaseModel) HistoryList.SelectedItem;
            PhaseName.Text = phase.Name;
            OriginalTest.Text = phase.CellTestName;
            NumTriggers.Text = phase.Triggers.Count.ToString();
            NumWidgets.Text = phase.Widgets.Count.ToString();
            Widgets.ItemsSource = phase.Widgets.OrderBy(w => w.WidgetId).Select(w => WidgetService.GetWidgetName(w.WidgetId));
            CopyPhase.IsEnabled = true;
        }

        private void CopyPhase_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PhaseToCopy = (CellTestPhaseModel) HistoryList.SelectedItem;
            Close();
        }
    }
}
