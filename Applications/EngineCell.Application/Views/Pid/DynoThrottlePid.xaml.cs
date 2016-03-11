using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EngineCell.Application.ViewModels.Pid;

namespace EngineCell.Application.Views.Pid
{
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

        private void ToggleAuto(object sender, RoutedEventArgs e)
        {
            var type = ((ToggleButton)sender).Tag.ToString();
            switch (type)
            {
                case "Dyno":
                    ViewModel.Dyno.IsAuto = true;
                    break;
                case "Throttle":
                    ViewModel.Throttle.IsAuto = true;
                    break;
            }
        }

        private void ToggleManual(object sender, RoutedEventArgs e)
        {
            var type = ((ToggleButton)sender).Tag.ToString();
            switch (type)
            {
                case "Dyno":
                    ViewModel.Dyno.IsAuto = false;
                    break;
                case "Throttle":
                    ViewModel.Throttle.IsAuto = false;
                    break;
            }
        }

        private void ModeSelection(object sender, RoutedEventArgs e)
        {
            var modeSelectModal = new PidModeSelection(ViewModel.Dyno, ViewModel.Throttle);
            modeSelectModal.ShowDialog();
        }
    }
}
