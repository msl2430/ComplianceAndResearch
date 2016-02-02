using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private TimeSpan RemainingTime { get; set; }
        
        public TimeRemaining()
        {
            InitializeComponent();
            RemainingTimeViewModel = new RemainingTimeViewModel();
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
            }
            else
            {
                if (RemainingTime != TimeSpan.Zero)
                    RemainingTime = TimeSpan.Zero;
            }
        }
    }
}
