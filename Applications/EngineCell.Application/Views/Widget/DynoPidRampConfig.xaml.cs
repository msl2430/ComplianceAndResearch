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
    /// Interaction logic for DynoPidRampConfig.xaml
    /// </summary>
    public partial class DynoPidRampConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public DynoPidRampConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();

            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.DynoRamp);

            if (Widget != null && Widget.Settings.IsNotNullOrEmpty())
            {
                PidRampTimeText.Text = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime)
                    ? Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime).Value).ToString()
                    : "1";
                PidSetpointText.Text = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint)
                    ? Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint).Value
                    : "300";
            }

            UpdateWidgetStatus();
        }

        private void PidSetpointText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var setpoint = ((TextBox) sender).Text;

            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint).Value = setpoint;
            else 
                Widget.Settings.Add(new WidgetSettingModel() {CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.DynoRampSetpoint, Value = setpoint });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.DynoRampSetpoint, setpoint);
            UpdateWidgetStatus();
        }

        private void PidRampTimeText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var rampTime = ((TextBox)sender).Text;

            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime).Value = rampTime;
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.DynoRampTime, Value = rampTime });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.DynoRampTime, rampTime);
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampTime) && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.DynoRampSetpoint))
            {
                WidgetStatus.Text = "Configured";
                WidgetStatus.Foreground = (Brush) FindResource("GreenBrush");
            }
            else
            {
                WidgetStatus.Text = "Not Configured";
                WidgetStatus.Foreground = Brushes.OrangeRed;
            }
        }
    }
}
