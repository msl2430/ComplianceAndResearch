using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.PointConfiguration;

namespace EngineCell.Application.Views.PointConfiguration
{
    /// <summary>
    /// Interaction logic for PointConfiguration.xaml
    /// </summary>
    public partial class PointConfiguration : UserControl
    {
        public PointConfigurationViewModel ViewModel { get; set; }

        public PointConfiguration()
        {
            InitializeComponent();
            
        }

        private void PointConfiguration_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (PointConfigurationViewModel) DataContext;
        }
    }
}
