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
    /// Interaction logic for StarterConfig.xaml
    /// </summary>
    public partial class StarterConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        public StarterConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();

            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.Starter);

            if (Widget != null && Widget.Settings.IsNotNullOrEmpty())
            {
                foreach (var setting in Widget.Settings)
                {
                    switch (setting.WidgetSettingId)
                    {
                        case WidgetConstants.WidgetSetting.StarterCrankTime:
                            CrankTimeText.Text = setting.Value;
                            break;
                        case WidgetConstants.WidgetSetting.StarterStartRpm:
                            StartRpmText.Text = setting.Value;
                            break;
                        case WidgetConstants.WidgetSetting.StarterTimeBetweenTries:
                            TimeBetweenTriesText.Text = setting.Value;
                            break;
                        case WidgetConstants.WidgetSetting.StarterNumberOfTries:
                            NumberOfTriesText.Text = setting.Value;
                            break;
                        case WidgetConstants.WidgetSetting.StarterTimeAtRpm:
                            CutoffTime.Text = setting.Value;
                            break;
                    }
                }
            }

            UpdateWidgetStatus();
        }

        private void CrankTimeText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetSetting(WidgetConstants.WidgetSetting.StarterCrankTime, ((TextBox) sender).Text);
        }

        private void StartRpmText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetSetting(WidgetConstants.WidgetSetting.StarterStartRpm, ((TextBox)sender).Text);
        }

        private void TimeBetweenTriesText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetSetting(WidgetConstants.WidgetSetting.StarterTimeBetweenTries, ((TextBox)sender).Text);
        }

        private void NumberOfTriesText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetSetting(WidgetConstants.WidgetSetting.StarterNumberOfTries, ((TextBox)sender).Text);
        }

        private void CutoffTime_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetSetting(WidgetConstants.WidgetSetting.StarterTimeAtRpm, ((TextBox)sender).Text);
        }

        private void SetSetting(WidgetConstants.WidgetSetting setting, string value)
        {
            if (Widget.Settings.Any(s => s.WidgetSettingId == setting))
                Widget.Settings.First(s => s.WidgetSettingId == setting).Value = value;
            else
                Widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = Widget.CellTestPhaseWidgetId, WidgetSettingId = setting, Value = value });

            WidgetRepository.SaveWidgetSetting(Widget.CellTestPhaseWidgetId, setting, value);
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterCrankTime) 
                && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterStartRpm)
                && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeBetweenTries)
                && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterNumberOfTries)
                && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.StarterTimeAtRpm))
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
