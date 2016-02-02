using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;
using EngineCell.Application.ViewModels.Clocks;

namespace EngineCell.Application.Views.Clocks
{
    /// <summary>
    /// Interaction logic for RunTime.xaml
    /// </summary>
    public partial class RunTime : UserControl
    {
        public RunTimeViewModel RunTimeViewModel { get; set; }

        public bool IsRunning { get; set; }

        private Stopwatch Stopwatch { get; set; }

        public RunTime()
        {
            InitializeComponent();
            RunTimeViewModel = new RunTimeViewModel();
            DataContext = RunTimeViewModel;
            Stopwatch = new Stopwatch();
            var timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            timer.Tick += (s, e) => { UpdateRunTime(); };
        }

        private void UpdateRunTime()
        {
            if (IsRunning)
            {
                if (!Stopwatch.IsRunning)
                    Stopwatch.Restart();
                RunTimeViewModel.RunTime = $"{Stopwatch.Elapsed.Hours.ToString("00")}:{Stopwatch.Elapsed.Minutes.ToString("00")}:{Stopwatch.Elapsed.Seconds.ToString("00")}";
            }
            else
            {
                if (!Stopwatch.IsRunning) return;
                Stopwatch.Reset();
                Stopwatch.Stop();
            }
        }
    }
}
