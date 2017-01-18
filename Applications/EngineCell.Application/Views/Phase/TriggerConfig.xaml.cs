using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Phase
{
    /// <summary>
    /// Interaction logic for TriggerConfig.xaml
    /// </summary>
    public partial class TriggerConfig : UserControl
    {
        public TriggerConfigViewModel ViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public TriggerConfig()
        {
            InitializeComponent();
        }

        private void TriggerConfig_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext.GetType() != typeof(TriggerConfigViewModel))
                return;
            if(DataContext != null)
                ViewModel = (TriggerConfigViewModel)DataContext;
            
            UpdateTriggerDisplay();
        }

        private void UpdateTriggerDisplay()
        {
            PointComboBox.ItemsSource = ViewModel.ApplicationSessionFactory.CellPoints.Where(p => ViewModel.Triggers.IsNullOrEmpty() || ViewModel.Triggers.All(t => t.CellPointId != p.CellPointId)).OrderBy(p => p.PointGroupId).ThenBy(p => p.CellPointId);
            var triggers = new List<Grid>();

            foreach (var trigger in ViewModel.Phase.Triggers)
            {
                var triggerGrid = new Grid()
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition() {Width = new GridLength(150)},
                        new ColumnDefinition() {Width = new GridLength(50)},
                        new ColumnDefinition() {Width = new GridLength(50)},
                        new ColumnDefinition() {Width = new GridLength(75)},
                        new ColumnDefinition() {Width = new GridLength(100)},
                        new ColumnDefinition() {Width = new GridLength(90)},
                        new ColumnDefinition() {Width = new GridLength(39)},
                        new ColumnDefinition() {Width = new GridLength(37)}
                    }
                };
                var pointLabel = GetTriggerLabel(trigger.CellPointName);
                var lowValueLabel = GetTriggerLabel(trigger.LowValue.ToString());
                var highValueLabel = GetTriggerLabel(trigger.HighValue.ToString());
                var thresholdLabel = GetTriggerLabel(trigger.SecondsThreshold.ToString());
                var actionLabel = GetTriggerLabel(trigger.ResultTypeId == WidgetConstants.TriggerResultType.GoToPhase ? "Go To Phase" : "Shutdown");
                var actionParameterLabel = GetTriggerLabel(trigger.ResultTypeParameter);
                var email = GetTriggerLabel(trigger.IsEmail ? "Yes" : "No");
                var removeButton = new Button() { Content = "X", FontSize = 8, Height = 7, Width = 25, Padding = new Thickness(3), Background = Brushes.OrangeRed, Margin = new Thickness(3), Tag = trigger.CellTestPhaseTriggerId};
                removeButton.Click += RemmoveTriggerButton_OnClick;

                Grid.SetColumn(pointLabel, 0);
                Grid.SetColumn(lowValueLabel, 1);
                Grid.SetColumn(highValueLabel, 2);
                Grid.SetColumn(thresholdLabel, 3);
                Grid.SetColumn(actionLabel, 4);
                Grid.SetColumn(actionParameterLabel, 5);
                Grid.SetColumn(email, 6);
                Grid.SetColumn(removeButton, 7);

                triggerGrid.Children.Add(pointLabel);
                triggerGrid.Children.Add(lowValueLabel);
                triggerGrid.Children.Add(highValueLabel);
                triggerGrid.Children.Add(thresholdLabel);
                triggerGrid.Children.Add(actionLabel);
                triggerGrid.Children.Add(actionParameterLabel);
                triggerGrid.Children.Add(email);
                triggerGrid.Children.Add(removeButton);

                triggers.Add(triggerGrid);
            }

            ActiveTriggers.Children.Clear();
            foreach (var trigger in triggers)
            {
                ActiveTriggers.Children.Add(trigger);
            }
        }

        private Label GetTriggerLabel(string label)
        {
            return new Label() { FontSize = 10, BorderThickness = new Thickness(0, 0, 0, 1.01), BorderBrush = Brushes.WhiteSmoke, Content = label };
        }

        private void CreateTriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsAddTrigger = true;
            NewTriggerGrid.Visibility = Visibility.Visible;
        }

        private void CancelNewTriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            LowValue.Text = "";
            HighValue.Text = "";
            SecondsThreshold.Text = "";
            ActionParamter.Text = "";
            PointComboBox.SelectedIndex = -1;
            ResultComboBox.SelectedIndex = 0;
            IsEmail.IsChecked = false;
            ViewModel.IsAddTrigger = false;
            NewTriggerGrid.Visibility = Visibility.Collapsed;
            ViewModel.IsAddTrigger = false;
            NewTriggerGrid.Visibility = Visibility.Collapsed;
        }

        private void AddTriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            decimal test;
            if (PointComboBox.SelectedIndex < 0
                || (string.IsNullOrEmpty(LowValue.Text) && decimal.TryParse(LowValue.Text, out test))
                || (string.IsNullOrEmpty(HighValue.Text) && decimal.TryParse(HighValue.Text, out test))
                || (ResultComboBox.SelectedIndex == 0 && string.IsNullOrEmpty(ActionParamter.Text)))
            {
                //TODO: SHow error msg
                return;
            }

            var newTrigger = WidgetRepository.AddTriggerToPhase(new CellTestPhaseTriggerModel()
            {
                CellTestPhaseId = ViewModel.Phase.CellTestPhaseId,
                CellPointId = ((PointDataModel) PointComboBox.SelectedItem).CellPointId,
                CellPointName = ((PointDataModel)PointComboBox.SelectedItem).CustomName,
                LowValue = Convert.ToDecimal(LowValue.Text),
                HighValue = Convert.ToDecimal(HighValue.Text),
                SecondsThreshold = string.IsNullOrEmpty(SecondsThreshold.Text) ? (int?) null : Convert.ToInt32(SecondsThreshold.Text),
                ResultTypeId = (WidgetConstants.TriggerResultType) (PointComboBox.SelectedIndex + 1),
                ResultTypeParameter = ActionParamter.Text,
                IsEmail = IsEmail.IsChecked.HasValue ? IsEmail.IsChecked.Value : false,
            });

            ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases.First(p => p.CellTestPhaseId == ViewModel.Phase.CellTestPhaseId).Triggers.Add(newTrigger);

            CancelNewTriggerButton_OnClick(null, null);

            UpdateTriggerDisplay();
        }

        private void RemmoveTriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            var triggerId = Convert.ToInt32(((Button) sender).Tag);

            var trigger = ViewModel.Triggers.FirstOrDefault(t => t.CellTestPhaseTriggerId == triggerId);
            if (trigger == null)
                return;

            WidgetRepository.RemoveTriggerFromPhase(triggerId);

            ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases.First(p => p.CellTestPhaseId == ViewModel.Phase.CellTestPhaseId)
                .Triggers.Remove(ViewModel.ApplicationSessionFactory.CurrentCellTest.Phases.First(p => p.CellTestPhaseId == ViewModel.Phase.CellTestPhaseId).Triggers.First(t => t.CellTestPhaseTriggerId == triggerId));

            UpdateTriggerDisplay();
        }
    }
}
