using System;
using System.Configuration;
using Robot.Application.Extensions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Robot.Core.Constants;

namespace Robot.Application.Views.Configuration
{
    /// <summary>
    /// Interaction logic for ConfigurationFlyout.xaml
    /// </summary>
    public partial class ConfigurationFlyout : BaseUserControl
    {
        public ConfigurationFlyout()
        {
            InitializeComponent();
            RollDiameter.Text = Properties.Settings.Default.RollDiameter.ToString();
        }

        private void RollDiameter_OnPreview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void RollDiameter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NumericTextbox.ValidateNumber(RollDiameter.Text))
            {
                SaveSettingsButton.IsEnabled = false;
                RollDiameter.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                SaveSettingsButton.IsEnabled = true;
                RollDiameter.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            decimal rolLDiameter;
            if (decimal.TryParse(RollDiameter.Text, out rolLDiameter))
            {
                Properties.Settings.Default.RollDiameter = rolLDiameter;
                Properties.Settings.Default.Save();
                ((Flyout)Parent).Tag = ControlConstants.ChangeTracking.Dirty;
                ((Flyout)Parent).IsOpen = false;
            }
            else
            {
                SaveSettingsButton.IsEnabled = false;
                RollDiameter.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
        }
    }
}
