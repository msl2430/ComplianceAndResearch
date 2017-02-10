using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for ThrottlePidRampConfig.xaml
    /// </summary>
    public partial class ThrottlePidRampConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public ThrottlePidRampConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();

            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.ThrottleRamp);

            if (Widget != null && Widget.Settings.IsNotNullOrEmpty())
            {
                PidRampTimeText.Text = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime)
                    ? Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime).Value).ToString()
                    : "1";
                PidSetpointText.Text = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint)
                    ? Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint).Value
                    : "300";
                PidMode.SelectedIndex = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode)
                    ? Convert.ToInt32(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode).Value)
                    : 0;
            }

            if (Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.ThrottleRampMode))
            {
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.ThrottleRampMode, Value = "0" });
                WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.ThrottleRampMode, "0");
            }


            UpdateWidgetStatus();
        }

        private void PidSetpointText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var setpoint = ((TextBox)sender).Text;

            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint).Value = setpoint;
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.ThrottleRampSetpoint, Value = setpoint });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.ThrottleRampSetpoint, setpoint);
            UpdateWidgetStatus();
        }

        private void PidRampTimeText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var rampTime = ((TextBox)sender).Text;

            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime).Value = rampTime;
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.ThrottleRampTime, Value = rampTime });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.ThrottleRampTime, rampTime);
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampTime) && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampSetpoint))
            {
                WidgetStatus.Text = "Configured";
                WidgetStatus.Foreground = (Brush)FindResource("GreenBrush");
            }
            else
            {
                WidgetStatus.Text = "Not Configured";
                WidgetStatus.Foreground = Brushes.OrangeRed;
            }
        }

        private void PidMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mode = ((ComboBox)sender).SelectedIndex;
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.ThrottleRampMode).Value = mode == 0 ? WidgetConstants.DynoPidMeasurement.Rpm.ToInt().ToString() : WidgetConstants.DynoPidMeasurement.Load.ToInt().ToString();
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.ThrottleRampMode, Value = mode == 0 ? WidgetConstants.DynoPidMeasurement.Rpm.ToInt().ToString() : WidgetConstants.DynoPidMeasurement.Load.ToInt().ToString() });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.ThrottleRampMode, mode == 0 ? WidgetConstants.DynoPidMeasurement.Rpm.ToInt().ToString() : WidgetConstants.DynoPidMeasurement.Load.ToInt().ToString());
        }
    }
}
