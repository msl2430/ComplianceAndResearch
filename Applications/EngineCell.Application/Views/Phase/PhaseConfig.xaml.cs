using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.Views.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Core.Models;
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
            PhaseTabs.SelectedIndex = 0;           
        }

        private void UpdatePhaseTabs(int? selectedIndex = null)
        {
            var tempIndex = selectedIndex == null ? PhaseTabs.SelectedIndex : Convert.ToInt32(selectedIndex);
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
            UpdatePhaseTabs(ViewModel.Phases.IndexOf(newPhase));
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
            WidgetConstants.Widget? widgetType = null;
            switch (selectedWidget)
            {
                case "Test Schedule":
                    widgetType = WidgetConstants.Widget.TestSchedule;
                    break;
                case "Dyno Ramp":
                    widgetType = WidgetConstants.Widget.DynoRamp;
                    break;
                case "Throttle Ramp":
                    widgetType = WidgetConstants.Widget.ThrottleRamp;
                    break;
                case "Custom Chart 1":
                    widgetType = WidgetConstants.Widget.CustomChart1;
                    break;
                case "Custom Chart 2":
                    widgetType = WidgetConstants.Widget.CustomChart2;
                    break;
                case "Custom Chart 3":
                    widgetType = WidgetConstants.Widget.CustomChart3;
                    break;
                case "Custom Chart 4":
                    widgetType = WidgetConstants.Widget.CustomChart4;
                    break;
                case "Custom Chart 5":
                    widgetType = WidgetConstants.Widget.CustomChart5;
                    break;
            }
            var widget = WidgetRepository.AddWidgetToPhase(phase.CellTestPhaseId, (WidgetConstants.Widget)widgetType);
            phase.Widgets.Add(widget);
            UpdatePhaseWidgetDisplay();
        }

        private void PhaseTabs_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource.GetType() != typeof(TabControl) || ((TabControl) e.OriginalSource).Name != "PhaseTabs")
                return;
            UpdatePhaseWidgetDisplay();
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var cp = PhaseTabs.Template.FindName("PART_SelectedContentHost", PhaseTabs) as ContentPresenter;
                try
                {
                    var g = PhaseTabs.ContentTemplate.FindName("TriggerConfig", cp) as Grid;
                    g.Children.Clear();
                    var triggerWindow = new TriggerConfig() {DataContext = new TriggerConfigViewModel(ViewModel.ApplicationSessionFactory, ViewModel.Phases.ElementAt(PhaseTabs.SelectedIndex), ViewModel.Phases.Select(p => new IdNamePair(p.CellTestPhaseId, p.Name)).ToList()) };
                    g.Children.Add(triggerWindow);
                }
                catch (Exception) { }
            }));
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
            if (ViewModel.Phases.IsNullOrEmpty())
                return;

            if (PhaseTabs.SelectedIndex < 0)
                PhaseTabs.SelectedIndex = 0;

            var widgets = WidgetConstants.Widgets.ToList();
            var phase = ViewModel.Phases.ElementAt(PhaseTabs.SelectedIndex);
            foreach (var phaseWidgets in phase.Widgets)
            {
                switch (phaseWidgets.WidgetId)
                {
                    case WidgetConstants.Widget.TestSchedule:
                        if(widgets.Any(w => w == "Test Schedule"))
                            widgets.RemoveAt(widgets.IndexOf("Test Schedule"));
                        break;
                    case WidgetConstants.Widget.DynoRamp:
                        if (widgets.Any(w => w == "Dyno Ramp"))
                            widgets.RemoveAt(widgets.IndexOf("Dyno Ramp"));
                        break;
                    case WidgetConstants.Widget.ThrottleRamp:
                        if (widgets.Any(w => w == "Throttle Ramp"))
                            widgets.RemoveAt(widgets.IndexOf("Throttle Ramp"));
                        break;
                    case WidgetConstants.Widget.CustomChart1:
                        if (widgets.Any(w => w == "Custom Chart 1"))
                            widgets.RemoveAt(widgets.IndexOf("Custom Chart 1"));
                        break;
                    case WidgetConstants.Widget.CustomChart2:
                        if (widgets.Any(w => w == "Custom Chart 2"))
                            widgets.RemoveAt(widgets.IndexOf("Custom Chart 2"));
                        break;
                    case WidgetConstants.Widget.CustomChart3:
                        if (widgets.Any(w => w == "Custom Chart 3"))
                            widgets.RemoveAt(widgets.IndexOf("Custom Chart 3"));
                        break;
                    case WidgetConstants.Widget.CustomChart4:
                        if (widgets.Any(w => w == "Custom Chart 4"))
                            widgets.RemoveAt(widgets.IndexOf("Custom Chart 4"));
                        break;
                    case WidgetConstants.Widget.CustomChart5:
                        if (widgets.Any(w => w == "Custom Chart 5"))
                            widgets.RemoveAt(widgets.IndexOf("Custom Chart 5"));
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
                UserControl configWindow = null;
                switch (phase.Widgets[i].WidgetId)
                {
                    case WidgetConstants.Widget.TestSchedule:
                        configWindow = new TestScheduleConfig(phase);
                        break;
                    case WidgetConstants.Widget.DynoRamp:
                        configWindow = new DynoPidRampConfig(phase);
                        break;
                    case WidgetConstants.Widget.ThrottleRamp:
                        //configWindow = new ThrottlePidRampConfig(phase);
                        break;
                    case WidgetConstants.Widget.CustomChart1:
                        configWindow = new CustomChartConfig(phase, WidgetConstants.Widget.CustomChart1);
                        break;
                    case WidgetConstants.Widget.CustomChart2:
                        configWindow = new CustomChartConfig(phase, WidgetConstants.Widget.CustomChart2);
                        break;
                    case WidgetConstants.Widget.CustomChart3:
                        configWindow = new CustomChartConfig(phase, WidgetConstants.Widget.CustomChart3);
                        break;
                    case WidgetConstants.Widget.CustomChart4:
                        configWindow = new CustomChartConfig(phase, WidgetConstants.Widget.CustomChart4);
                        break;
                    case WidgetConstants.Widget.CustomChart5:
                        configWindow = new CustomChartConfig(phase, WidgetConstants.Widget.CustomChart5);
                        break;
                }

                if(configWindow == null)
                    continue;
                
                var removeButton = new Button() { Width = 150, Height = 30, HorizontalAlignment = HorizontalAlignment.Center, Background = Brushes.Red, Content = "Remove", Tag = phase.Widgets[i].CellTestPhaseWidgetId };
                removeButton.Click += RemoveWidgetFromPhase;
                Grid.SetRow(configWindow, 0);
                Grid.SetRow(removeButton, 1);
                grid.Children.Add(configWindow);
                grid.Children.Add(removeButton);
                border.Child = grid;

                Grid.SetRow(border, 0);
                Grid.SetColumn(border, i);
                
                widgetGrid.Children.Add(border);
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var cp = PhaseTabs.Template.FindName("PART_SelectedContentHost", PhaseTabs) as ContentPresenter;
                try
                {
                    var g = PhaseTabs.ContentTemplate.FindName("WidgetConfigGrid", cp) as Grid;
                    g.Children.Clear();
                    g.Children.Add(widgetGrid);
                }
                catch (Exception) { }
            }));
            

            ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases = ViewModel.Phases;
        }        
    }
}
