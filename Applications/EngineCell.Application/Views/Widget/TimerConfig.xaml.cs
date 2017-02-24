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
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for TimerConfig.xaml
    /// </summary>
    public partial class TimerConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public TimerConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();

            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.Timer);

            if (Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.TimerExpiration))
            {
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.TimerExpiration, Value = "300" });
                WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.TimerExpiration, "300");
            }

            if (Widget != null && Widget.Settings.IsNotNullOrEmpty())
            {
                TimerExpirationText.Text = Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration)
                    ? Convert.ToInt64(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration).Value).ToString()
                    : "300";
            }
            
            UpdateWidgetStatus();
        }

        private void TimerExpirationText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var expiration = ((TextBox)sender).Text;

            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration))
                Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration).Value = expiration;
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.TimerExpiration, Value = expiration });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.TimerExpiration, expiration);
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TimerExpiration))
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
    }
}
