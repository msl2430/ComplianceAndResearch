using EngineCell.Application.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
        }

        private void PidConfiguration_OnLoaded(object sender, RoutedEventArgs e)
        {
            Settings = (PidSetting)DataContext;
        }
    }
}
