﻿using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.Pid;

namespace EngineCell.Application.Views.Pid
{
    public partial class PidDisplay : UserControl
    {
        public PidDisplayModel PidConfig { get; set; }        

        public PidDisplay()
        {
            InitializeComponent();
        }

        private void PidDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            PidConfig = (PidDisplayModel)DataContext;
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
    }
}
