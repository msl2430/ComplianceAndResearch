using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for TimerDisplay.xaml
    /// </summary>
    public partial class TimerDisplay : UserControl
    {
        private TimerDisplayViewModel ViewModel { get; set; }

        private Stopwatch Timer { get; set; }

        private bool IsRunning { get; set; }

        public TimerDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            widget.IsRunning = true;
            ViewModel = new TimerDisplayViewModel(appSession, widget);
            Timer = new Stopwatch();
            ViewModel.CurrentTime = "00:00:00";
        }

        private void TimerDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;

            Timer.Start();
            IsRunning = true;

            Task.Run(() =>
            {
                while (IsRunning)
                {
                    var timespan = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(Timer.ElapsedMilliseconds));
                    ViewModel.CurrentTime = timespan.Hours.ToString("0#") + ":" + timespan.Minutes.ToString("0#") + ":" + timespan.Seconds.ToString("0#");
                    CheckTimer();
                    Thread.Sleep(100);
                }
            });
        }

        private void CheckTimer()
        {
            if (Timer.ElapsedMilliseconds/1000 < ViewModel.ExpirationTimeSeconds) return;

            ViewModel.Widget.IsComplete = true;
            IsRunning = false;
            Timer.Stop();
        }
    }
}
