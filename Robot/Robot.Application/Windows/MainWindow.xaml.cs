using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Opto22.Core.Constants;
using Robot.Application.Factories;
using Robot.Application.Services;
using Robot.Application.Session;

namespace Robot.Application.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeWatchThreads();
            InitializeDataWorkerService = new InitializeDataWorkerService(PointService, Dispatcher, PointListBox);
            Task.Run(() => InitializeDataWorkerService.DoWork()).ConfigureAwait(false);
        }

        private void InitializeWatchThreads()
        {
            OptoConnectionWorker = new OptoConnectionWorkerService(OptoMmpFactory, Dispatcher, OptoConnectionStatusLabel);
            Task.Run(() => OptoConnectionWorker.DoWork()).ConfigureAwait(false);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ExitThreads();
        }

        private void DisconnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            ExitThreads();
        }

        private void ExitThreads()
        {
            if (OptoConnectionWorker.IsRunning)
                OptoConnectionWorker.CancelWork();
        }

        private void PointListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicationSession.AnalogPoints == null ||
                ApplicationSession.AnalogPoints.All(ap => string.Concat("Point", ap.PointNumber) != ((ListBoxItem) PointListBox.SelectedItem).Name.ToString())) 
                return;

            var point = ApplicationSession.AnalogPoints.FirstOrDefault(ap => string.Concat("Point", ap.PointNumber) == ((ListBoxItem)PointListBox.SelectedItem).Name.ToString());
            PointNumberTextBox.Text = point.PointNumber.ToString();
            PointNameTextBox.Text = point.PointName;
            PointTypeTextBox.Text = Enum.GetName(typeof (Opto22Constants.PointType), point.PointType);
            ModuleTypeTextBox.Text = Enum.GetName(typeof(Opto22Constants.ModuleType), point.ModuleType);
            HighScaleTextBox.Text = point.HighScale.ToString();
            LowScaleTextBox.Text = point.LowScale.ToString();
            GainTextBox.Text = point.Gain.ToString();
            FilterWeightTextBox.Text = point.FilterWeight.ToString();
            OffsetTextBox.Text = point.Offset.ToString();
            EnableWatchdogCheckbox.IsChecked = point.EnableWatchdog;
            WatchdogValueTextBox.Text = point.WatchdogValue.ToString();
        }
    }
}
