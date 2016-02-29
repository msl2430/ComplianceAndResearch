using System;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels;
using EngineCell.Application.ViewModels.Pid;

namespace EngineCell.Application.Views.Pid
{
    /// <summary>
    /// Interaction logic for PidConfiguration.xaml
    /// </summary>
    public partial class PidConfigurationOld : UserControl
    {
        public PidSettingOld SettingsOld { get; set; }

        public PidConfigurationOld()
        {
            InitializeComponent();
        }

        private void SetButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsOld.IsDirty = false;
            SettingsOld.Output = Convert.ToDecimal(((TextBox)sender).Text);
        }

        private void PidConfiguration_OnLoaded(object sender, RoutedEventArgs e)
        {
            SettingsOld = (PidSettingOld)DataContext;
        }

        private void Output_OnChange(object sender, TextChangedEventArgs e)
        {
            if (sender == null || SettingsOld == null)
                return;
            SettingsOld.IsDirty = true;
        }
    }
}
