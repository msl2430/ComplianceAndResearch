using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Core.Constants;
using EngineCell.Models.Repositories;
using MahApps.Metro.Controls;

namespace EngineCell.Application.Views.Phase
{
    /// <summary>
    /// Interaction logic for PhaseConfig.xaml
    /// </summary>
    public partial class PhaseConfig : UserControl
    {
        public PhaseConfigViewModel ViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public PhaseConfig()
        {
            InitializeComponent();            
        }

        private void PhaseConfig_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (PhaseConfigViewModel) DataContext;

            AvailableWidgets.ItemsSource = WidgetConstants.Widgets;
            PhaseTabs.ItemsSource = ViewModel.Phases.OrderBy(p => p.PhaseOrder).ToList();
            PhaseTabs.SelectedIndex = 0;
        }

        private void UpdatePhaseTabs()
        {
            var tempIndex = PhaseTabs.SelectedIndex;
            PhaseTabs.ItemsSource = ViewModel.Phases.OrderBy(p => p.PhaseOrder).ToList();
            PhaseTabs.SelectedIndex = tempIndex;
        }

        private void AddPhase(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewPhaseName.Text) || ViewModel.Phases.Any(p => p.Name == NewPhaseName.Text))
            {
                NewPhaseError.Visibility = Visibility.Visible;
                return;
            }
            NewPhaseError.Visibility = Visibility.Collapsed;

            var newPhase = WidgetRepository.AddPhaseToTest(ViewModel.ApplicationSessionFactory.CurrentCellTest.CellTestId, ViewModel.Phases.Count + 1, NewPhaseName.Text);
            ViewModel.Phases.Add(newPhase);

            UpdatePhaseTabs();
        }

        private void UpdatePhase(object sender, RoutedEventArgs e)
        {
            var cellTestPhaseId = Convert.ToInt32(((Button) sender).Tag);
            var phaseName = PhaseTabs.FindChild<TextBox>("PhaseName")?.Text ?? "";
            if (string.IsNullOrEmpty(phaseName))
                return;

            WidgetRepository.UpdatePhase(cellTestPhaseId, phaseName);
            var phase = ViewModel.Phases.FirstOrDefault(p => p.CellTestPhaseId == cellTestPhaseId);
            if (phase == null)
                return;

            phase.Name = phaseName;
            UpdatePhaseTabs();
        }        

        private void DeletePhase(object sender, RoutedEventArgs e)
        {
            var cellTestPhaseId = Convert.ToInt32(((Button)sender).Tag);
            var phase = ViewModel.Phases.FirstOrDefault(p => p.CellTestPhaseId == cellTestPhaseId);
            if (phase == null)
                return;

            WidgetRepository.DeletePhaseFromTest(ViewModel.ApplicationSessionFactory.CurrentCellTest.CellTestId, cellTestPhaseId);
            var index = phase.PhaseOrder;
            ViewModel.Phases.Remove(phase);
            foreach (var p in ViewModel.Phases)
            {
                if (p.PhaseOrder > index)
                    p.PhaseOrder--;
            }
            UpdatePhaseTabs();
        }
    }
}
