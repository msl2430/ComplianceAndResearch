using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for WidgetConfig.xaml
    /// </summary>
    public partial class WidgetConfig : BaseUserControl
    {
        public WidgetConfigViewModel ViewModel { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public WidgetConfig()
        {
            InitializeComponent();

            VentCtrl1Inside.ItemsSource = Enum.GetValues(typeof (ThermoCouple)).Cast<ThermoCouple>();
            VentCtrl1Outside.ItemsSource = Enum.GetValues(typeof(ThermoCouple)).Cast<ThermoCouple>();
            VentCtrl1Output.ItemsSource = Enum.GetValues(typeof(AnalogOutput)).Cast<AnalogOutput>();

            DynoMode.ItemsSource = Enum.GetValues(typeof(WidgetConstants.DynoPidMode)).Cast<WidgetConstants.DynoPidMode>();
            DynoMeasurement.ItemsSource = Enum.GetValues(typeof(WidgetConstants.DynoPidMeasurement)).Cast<WidgetConstants.DynoPidMeasurement>();

            AvailableWidgets.ItemsSource = new List<string>() {"Starter", "Dyno PID", "Ventilation Control 1"};
        }

        private void WidgetConfig_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (WidgetConfigViewModel)DataContext;

            GetWidgetConfigs();
        }

        private void GetWidgetConfigs() 
        {
            var ventCtrl1Settings = WidgetRepository.GetWidgetSettingByWidgetCell(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, WidgetConstants.Widget.VentilationControl1);
            //var dynoPidSettings = WidgetRepository.GetWidgetSettingByWidgetCell(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, WidgetConstants.Widget.DynoPid);
            var starterSettings = WidgetRepository.GetWidgetSettingByWidgetCell(ViewModel.ApplicationSessionFactory.CurrentCell.CellId, WidgetConstants.Widget.Starter);

            if (ventCtrl1Settings.IsNotNullOrEmpty())
                ViewModel.VentCtrl1.SetValues(ventCtrl1Settings);
            //if (dynoPidSettings.IsNotNullOrEmpty())
            //    ViewModel.DynoPid.SetValues(dynoPidSettings);
            //if (starterSettings.IsNotNullOrEmpty())
            //    ViewModel.Starter.SetValues(starterSettings);
        }

        private void ActivateVentCtrl1_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.VentCtrl1.IsActive = true;
        }

        private void DeactivateVentCtrl1_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.VentCtrl1.IsActive = false;
        }

        private void SaveVent1_OnClick(object sender, RoutedEventArgs e)
        {
            if (VentCtrl1Inside.SelectedItem == null || VentCtrl1Outside.SelectedItem == null || VentCtrl1Output.SelectedItem == null || string.IsNullOrEmpty(VentCtrl1Gain.Text) || string.IsNullOrEmpty(VentCtrl1SetPoint.Text))
            {
                return; //TODO: add error message
            }

            ViewModel.VentCtrl1.Inside = (ThermoCouple)VentCtrl1Inside.SelectedItem;
            ViewModel.VentCtrl1.Outside = (ThermoCouple)VentCtrl1Outside.SelectedItem;
            ViewModel.VentCtrl1.Output = (AnalogOutput)VentCtrl1Output.SelectedItem;
            ViewModel.VentCtrl1.Gain = Convert.ToDecimal(VentCtrl1Gain.Text);
            ViewModel.VentCtrl1.SetPoint = Convert.ToDecimal(VentCtrl1SetPoint.Text);

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1InsideType.ToInt(), Convert.ToInt32(VentCtrl1Inside.SelectedItem));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutsideType.ToInt(), Convert.ToInt32(VentCtrl1Outside.SelectedItem));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1OutputType.ToInt(), Convert.ToInt32(VentCtrl1Output.SelectedItem));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Setpoint.ToInt(), Convert.ToDecimal(VentCtrl1SetPoint.Text));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.VentCtrl1Gain.ToInt(), Convert.ToDecimal(VentCtrl1Gain.Text));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.VentCtrl1Status.ToInt(), ViewModel.VentCtrl1.IsActive ? 1 : 0);

            var vent1Settings = ViewModel.VentCtrl1.GetValues(ViewModel.ApplicationSessionFactory.CurrentCell.CellId);
            if (vent1Settings.IsNotNullOrEmpty()) 
                WidgetRepository.SaveWidgetSettings(vent1Settings);

            ViewModel.ApplicationSessionFactory.LogEvent("Ventilation Control 1 settings applied.", true);
        }

        private void ActivateDynoPid_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DynoPid.IsActive = true;
        }

        private void DeactivateDynoPid_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DynoPid.IsActive = false;
        }

        private void SaveDynoPid_OnClick(object sender, RoutedEventArgs e)
        {
            if (VentCtrl1Inside.SelectedItem == null || VentCtrl1Outside.SelectedItem == null || VentCtrl1Output.SelectedItem == null || string.IsNullOrEmpty(VentCtrl1Gain.Text) || string.IsNullOrEmpty(VentCtrl1SetPoint.Text))
            {
                return; //TODO: add error message
            }

            ViewModel.DynoPid.DynoMode = (WidgetConstants.DynoPidMode)DynoMode.SelectedItem;
            ViewModel.DynoPid.DynoMeasurement = (WidgetConstants.DynoPidMeasurement)DynoMeasurement.SelectedItem;
            ViewModel.DynoPid.DynoSetpoint = Convert.ToDecimal(DynoSetpoint.Text);

            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.DynoPidMode.ToInt(), Convert.ToInt32(DynoMode.SelectedItem));
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.DynoPidMeasurement.ToInt(), Convert.ToInt32(DynoMeasurement.SelectedItem));
            
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.DynoPidSetpoint.ToInt(), Convert.ToDecimal(DynoSetpoint.Text));
            
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.DynoPidStatus.ToInt(), ViewModel.DynoPid.IsActive ? 1 : 0);

            var dynoSettings = ViewModel.DynoPid.GetValues(ViewModel.ApplicationSessionFactory.CurrentCell.CellId);
            if (dynoSettings.IsNotNullOrEmpty())
                WidgetRepository.SaveWidgetSettings(dynoSettings);

            ViewModel.ApplicationSessionFactory.LogEvent("Dyno PID settings applied.", true);
        }

        private void ActivateStarter_OnClick(object sender, RoutedEventArgs e)
        {
            //ViewModel.Starter.IsActive = true;
        }

        private void DeactvateStarter_OnClick(object sender, RoutedEventArgs e)
        {
            //ViewModel.Starter.IsActive = false;
        }

        private void SaveStarter_OnClick(object sender, RoutedEventArgs e)
        {
            //ViewModel.Starter.CrankTime = Convert.ToInt32(CrankTime.Text);
            //ViewModel.Starter.StartParameter = Convert.ToDecimal(StartParameter.Text);
            //ViewModel.Starter.AddedCrankTime = Convert.ToInt32(AddedCrankTime.Text);

            //var starterSettings = ViewModel.Starter.GetValues(ViewModel.ApplicationSessionFactory.CurrentCell.CellId);
            //if (starterSettings.IsNotNullOrEmpty())
            //    WidgetRepository.SaveWidgetSettings(starterSettings);

            //ViewModel.ApplicationSessionFactory.LogEvent("Starter settings applied.", true);
        }
    }
}
