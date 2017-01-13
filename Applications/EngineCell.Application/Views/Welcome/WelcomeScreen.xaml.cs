using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Welcome;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;
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

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public WelcomeScreen()
        {
            InitializeComponent();
        }

        public void SetTest(CellTestModel test)
        {
            ViewModel.ApplicationSessionFactory.CurrentCellTest = test;
            ViewModel.MainWindow.UpdateViewModels();
            ViewModel.MainWindow.ChangePageView(ControlConstants.Views.TestDisplay);
        }

        private void WelcomeScreen_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (WelcomeScreenViewModel) DataContext;
        }

        private void NewTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new NewTestDialog();
            dialog.ShowDialog();

            if (string.IsNullOrEmpty(dialog.NewTestName))
                return;

            var test = CellPointRepository.CreateCellTest(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, dialog.NewTestName, dialog.NewTestDescription);
            SetTest(test);
        }

        private void PastTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new TestHistoryDialog(ViewModel.ApplicationSessionFactory);
            dialog.ShowDialog();

            if (dialog.ViewModel.TestToCopy == null)
                return;

            var test = CellPointRepository.CreateCellTest(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, dialog.ViewModel.TestToCopy.Name + " - Copy", dialog.ViewModel.TestToCopy.Description);
            foreach (var phase in dialog.ViewModel.TestToCopy.Phases)
            {
                var savedPhase = WidgetRepository.AddPhaseToTest(test.CellTestId, phase.PhaseOrder, phase.Name);
                foreach (var widget in phase.Widgets)
                {
                    var savedWidget = WidgetRepository.AddWidgetToPhase(savedPhase.CellTestPhaseId, widget.WidgetId);
                    foreach (var setting in widget.Settings)
                    {
                        var widgetSetting = WidgetRepository.SaveWidgetSetting(savedWidget.CellTestPhaseWidgetId, setting.WidgetSettingId, setting.Value);
                        savedWidget.Settings.Add(widgetSetting);
                    }
                    foreach (var setting in widget.PhaseEndSettings)
                    {
                        var endSetting = WidgetRepository.SavePhaseEndSetting(savedWidget.CellTestPhaseWidgetId, setting.PhaseEndSettingId, setting.Value);
                        savedWidget.PhaseEndSettings.Add(endSetting);
                    }
                    savedPhase.Widgets.Add(savedWidget);
                }
                test.Phases.Add(savedPhase);
            }
            SetTest(test);
        }
    }
}
