using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Core.Constants;

namespace EngineCell.Application.Views.Pid
{
    public partial class PidDisplay : UserControl
    {
        public PidDisplayViewModel ViewModel { get; set; }

        public PidDisplay()
        {
            InitializeComponent();
        }

        private void PidDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (PidDisplayViewModel)DataContext;
        }

        private void ToggleAuto(object sender, RoutedEventArgs e)
        {
            AutoButton.IsChecked = true;
            ManualButton.IsChecked = false;
        }

        private void ToggleManual(object sender, RoutedEventArgs e)
        {
            AutoButton.IsChecked = false;
            ManualButton.IsChecked = true;
        }

        private void ModeSelection_Click(object sender, RoutedEventArgs e)
        {
            var modeSelectModal = new PidModeSelection(ViewModel.PidConfig, null);
            modeSelectModal.ShowDialog();
        }

        private void ConfigPid_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: Testing commented out
            //if (ViewModel.ApplicationSessionFactory.OptoConnectionStatus != StatusConstants.ConnectionStatus.Connected)
            //    return;
            var pidConfig = new PidConfig(ViewModel.ApplicationSessionFactory, ViewModel.PidType);
            pidConfig.Show();
        }
    }
}
