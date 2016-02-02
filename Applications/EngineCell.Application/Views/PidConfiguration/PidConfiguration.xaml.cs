using System;
using EngineCell.Application.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EngineCell.Application.Views.PidConfiguration
{
    /// <summary>
    /// Interaction logic for PidConfiguration.xaml
    /// </summary>
    public partial class PidConfiguration : UserControl
    {
        public PidSetting Settings { get; set; }

        public PidConfiguration()
        {
            InitializeComponent();
        }

        private void SetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.IsDirty = false;
            Settings.Output = Convert.ToDecimal(((TextBox)sender).Text);
        }

        private void PidConfiguration_OnLoaded(object sender, RoutedEventArgs e)
        {
            Settings = (PidSetting)DataContext;
        }

        private void Output_OnChange(object sender, TextChangedEventArgs e)
        {
            if (sender == null || Settings == null)
                return;
            Settings.IsDirty = true;
        }
    }
}
