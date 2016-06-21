using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Widget;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for VentControlDisplay.xaml
    /// </summary>
    public partial class VentControlDisplay : UserControl
    {
        public VentilationControlDisplayViewModel ViewModel { get; set; }

        public VentControlDisplay()
        {
            InitializeComponent();
        }

        private void VentControlDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (VentilationControlDisplayViewModel)DataContext;
        }
    }
}
