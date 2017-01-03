using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Widget;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for StarterDisplay.xaml
    /// </summary>
    public partial class StarterDisplay : UserControl
    {
        public StarterDisplayViewModel ViewModel { get; set; }

        public StarterDisplay()
        {
            InitializeComponent();
        }

        private void StarterDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (StarterDisplayViewModel)DataContext;
        }
    }
}
