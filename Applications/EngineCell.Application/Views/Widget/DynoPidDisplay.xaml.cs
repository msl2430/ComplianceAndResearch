using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Widget;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for DynoPidDisplay.xaml
    /// </summary>
    public partial class DynoPidDisplay : UserControl
    {
        public DynoPidDisplayViewModel ViewModel { get; set; }

        public DynoPidDisplay()
        {
            InitializeComponent();
        }

        private void DynoPidDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (DynoPidDisplayViewModel)DataContext;
        }
    }
}
