using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Welcome;
using EngineCell.Core.Constants;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Welcome
{
    /// <summary>
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : UserControl
    {
        public WelcomeScreenViewModel ViewModel { get; set; }

        private ICellPointRepository _cellPointRepository { get; set; }
        private ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void WelcomeScreen_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (WelcomeScreenViewModel) DataContext;
        }

        private void NewTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var test = CellPointRepository.CreateCellTest(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, "Some test", "", ControlConstants.CellTestType.Manual);
            ViewModel.ApplicationSessionFactory.CurrentCellTest = test;
            ViewModel.MainWindow.ChangePageView(ControlConstants.Views.TestDisplay);
        }
    }
}
