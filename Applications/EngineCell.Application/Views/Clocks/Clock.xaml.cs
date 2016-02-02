using System;
using System.Windows.Controls;
using System.Windows.Threading;
using EngineCell.Application.ViewModels.Clocks;

namespace EngineCell.Application.Views.Clocks
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        public ClockViewModel ClockViewModel { get; set; }

        public Clock()
        {
            InitializeComponent();
            ClockViewModel = new ClockViewModel();
            DataContext = ClockViewModel;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.IsEnabled = true;
            timer.Tick += (s, e) =>
            {
                UpdateTime();
            };
        }

        private void UpdateTime()
        {
            ClockViewModel.TimeOfDay = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
