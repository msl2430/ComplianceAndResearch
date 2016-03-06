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
using EngineCell.Application.ViewModels.Pid;

namespace EngineCell.Application.Views.Pid
{
    /// <summary>
    /// Interaction logic for DynoThrottlePid.xaml
    /// </summary>
    public partial class DynoThrottlePid : UserControl
    {
        private DynoThrottlePidViewModel ViewModel { get; set; }

        public DynoThrottlePid()
        {
            InitializeComponent();
        }

        private void DynoThrottlePid_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (DynoThrottlePidViewModel)DataContext;
        }
    }
}
