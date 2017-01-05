using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.Views.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Models;
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
            UpdatePhaseWidgetDisplay();
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
            PhaseTabs.SelectedIndex = 0;
            UpdatePhaseTabs();
        }

        private void AvailableWidgets_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AvailableWidgets.SelectedItem == null)
                return;
            var selectedWidget = AvailableWidgets.SelectedItem.ToString();
            var phase = ViewModel.Phases.ElementAt(PhaseTabs.SelectedIndex);
            switch (selectedWidget)
            {
                case "Test Schedule":
                    var widget = WidgetRepository.AddWidgetToPhase(phase.CellTestPhaseId, WidgetConstants.Widget.TestSchedule);
                    phase.Widgets.Add(widget);
                    break;
            }
            UpdatePhaseWidgetDisplay();
        }

        private void PhaseTabs_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePhaseWidgetDisplay();
        }

        private void RemoveWidgetFromPhase(object sender, RoutedEventArgs e)
        {
            var cellTestPhaseWidgetId = Convert.ToInt32(((Button)sender).Tag);
            var phase = ViewModel.Phases.FirstOrDefault(p => p.CellTestPhaseId == ViewModel.Phases.ElementAt(PhaseTabs.SelectedIndex).CellTestPhaseId);
            if (phase == null)
                return;

            WidgetRepository.DeleteWidgetFromPhase(cellTestPhaseWidgetId);
            phase.Widgets.Remove(phase.Widgets.FirstOrDefault(w => w.CellTestPhaseWidgetId == cellTestPhaseWidgetId));
            UpdatePhaseWidgetDisplay();
        }

        private void UpdatePhaseWidgetDisplay()
        {
            var widgets = WidgetConstants.Widgets.ToList();
            var phase = ViewModel.Phases.ElementAt(PhaseTabs.SelectedIndex);
            foreach (var phaseWidgets in phase.Widgets)
            {
                switch (phaseWidgets.WidgetId)
                {
                    case WidgetConstants.Widget.TestSchedule:
                        widgets.RemoveAt(widgets.IndexOf("Test Schedule"));
                        break;
                    case WidgetConstants.Widget.VentilationControl1:
                        widgets.RemoveAt(widgets.IndexOf("Ventilation Control 1"));
                        break;
                    case WidgetConstants.Widget.VentilationControl2:
                        widgets.RemoveAt(widgets.IndexOf("Ventilation Control 2"));
                        break;
                    case WidgetConstants.Widget.DynoPid:
                        widgets.RemoveAt(widgets.IndexOf("Dyno PID"));
                        break;
                    case WidgetConstants.Widget.Starter:
                        widgets.RemoveAt(widgets.IndexOf("Starter"));
                        break;
                }
            }
            AvailableWidgets.ItemsSource = widgets;

            var widgetGrid = new Grid() { RowDefinitions = { new RowDefinition() { Height = new GridLength(350)} } };
            for (var i = 0; i < phase.Widgets.Count; i++)
            {
                widgetGrid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(270)});
                var border = new Border()
                {
                    BorderBrush = (Brush) FindResource("HeadlineBlueBrush"),
                    BorderThickness = new Thickness(1.01, 1.01, 1.01, 1.01),
                    Margin = new Thickness(5, 5, 5, 5),
                    Padding = new Thickness(5, 5, 5, 5)
                };
                var grid = new Grid() { RowDefinitions = { new RowDefinition() { Height = new GridLength(270)}, new RowDefinition() { Height = new GridLength(50)} }};
                var schedule = new TestScheduleConfig(phase);
                var removeButton = new Button() { Width = 150, Height = 30, HorizontalAlignment = HorizontalAlignment.Center, Background = Brushes.Red, Content = "Remove", Tag = phase.Widgets[0].CellTestPhaseWidgetId };
                removeButton.Click += RemoveWidgetFromPhase;
                Grid.SetRow(schedule, 0);
                Grid.SetRow(removeButton, 1);
                grid.Children.Add(schedule);
                grid.Children.Add(removeButton);
                border.Child = grid;

                Grid.SetRow(border, 0);
                Grid.SetColumn(border, i);
                
                widgetGrid.Children.Add(border);
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var cp = PhaseTabs.Template.FindName("PART_SelectedContentHost", PhaseTabs) as ContentPresenter;
                var g = PhaseTabs.ContentTemplate.FindName("WidgetConfigGrid", cp) as Grid;
                g.Children.Clear();
                g.Children.Add(widgetGrid);
            }));
            
        }       
    }
}
