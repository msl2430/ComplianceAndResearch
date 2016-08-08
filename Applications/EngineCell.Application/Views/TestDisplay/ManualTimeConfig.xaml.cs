using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using EngineCell.Application.ViewModels.TestDisplay;

namespace EngineCell.Application.Views.TestDisplay
{
    /// <summary>
    /// Interaction logic for ManualTimeConfig.xaml
    /// </summary>
    public partial class ManualTimeConfig : Window
    {
        public ManualTimeConfigViewModel ViewModel { get; set; }

        public ManualTimeConfig(ManualTimeConfigViewModel model)
        {
            InitializeComponent();
            ViewModel = model;
            DataContext = ViewModel;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            var hour = Convert.ToInt32(TestHour.Text);
            var min = Convert.ToInt32(TestMinute.Text);
            var second = Convert.ToInt32(TestSecond.Text);

            ViewModel.IsError = (hour > 12 || min > 60 || second > 60) || (hour == 0 && min == 0 && second == 0);

            if (ViewModel.IsError)
                return;

            ViewModel.Hour = hour;
            ViewModel.Minute = min;
            ViewModel.Second = second;
            ViewModel.IsCancel = false;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCancel = true;
            Close();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
