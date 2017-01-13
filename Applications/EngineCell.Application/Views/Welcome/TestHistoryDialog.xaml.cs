using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Welcome;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Welcome
{
    /// <summary>
    /// Interaction logic for TestHistoryDialog.xaml
    /// </summary>
    public partial class TestHistoryDialog : Window
    {
        public TestHistoryViewModel ViewModel { get; set; }

        private ICellPointRepository _cellPointRepository { get; set; }
        public ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

        public TestHistoryDialog(IApplicationSessionFactory appSession)
        {
            InitializeComponent();
            ViewModel = new TestHistoryViewModel() {ApplicationSessionFactory = appSession};
        }

        private void TestHistoryDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Tests = CellPointRepository.GetAllCellTests(ViewModel.ApplicationSessionFactory.CurrentCell != null ? (int?)ViewModel.ApplicationSessionFactory.CurrentCell.CellId : null);
            DataContext = ViewModel;
            CopyTest.IsEnabled = false;
        }

        private void HistoryList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = (CellTestModel) HistoryList.SelectedItem;
            TestName.Text = test.Name;
            TestCreationDate.Text = test.CreationDateString;
            TestCellName.Text = test.CellName;
            TestPhases.ItemsSource = test.Phases.OrderBy(p => p.PhaseOrder).Select(p => string.Format("{0} ({1})", p.Name, StringExtensions.Pluralize("1 Widget", "Widgets", "No Widgets", p.Widgets.Count)));
            CopyTest.IsEnabled = true;
        }

        private void CopyTest_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.TestToCopy = (CellTestModel) HistoryList.SelectedItem;
            Close();
        }
    }
}
