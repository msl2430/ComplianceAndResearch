using System;
using System.Windows.Controls;
using System.Windows.Threading;
using EngineCell.Application.ViewModels.Clocks;

namespace EngineCell.Application.Views.Clocks
{
    /// <summary>
    /// Interaction logic for TimeRemaining.xaml
    /// </summary>
    public partial class TimeRemaining : UserControl
    {
        public RemainingTimeViewModel RemainingTimeViewModel { get; set; }

        public bool IsRunning { get; set; }

        public TimeSpan RemainingTime { get; set; }
        
        public TimeRemaining()
        {
            InitializeComponent();
            RemainingTimeViewModel = new RemainingTimeViewModel() { IsVisible = false};
            DataContext = RemainingTimeViewModel;
            RemainingTime = TimeSpan.Zero;
            var timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            timer.Tick += (s, e) => { UpdateRemainingTime(); };
        }

        public void SetTimer(TimeSpan length)
        {
            RemainingTime = length;
            IsRunning = true;
        }

        public void UpdateRemainingTime()
        {
            if (IsRunning)
            {
                RemainingTimeViewModel.RemainingTime = $"{RemainingTime.Hours.ToString("00")}:{RemainingTime.Minutes.ToString("00")}:{RemainingTime.Seconds.ToString("00")}";

                if (RemainingTime == TimeSpan.Zero)
                    IsRunning = false;

                RemainingTime = RemainingTime.Add(TimeSpan.FromSeconds(-1));
                RemainingTimeViewModel.IsVisible = true;
            }
            else
            {
                if (RemainingTime != TimeSpan.Zero)
                    RemainingTime = TimeSpan.Zero;
            }
        }
    }
}
