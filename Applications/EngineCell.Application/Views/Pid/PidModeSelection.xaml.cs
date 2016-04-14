﻿using System;
using System.Windows;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Core.Constants;

namespace EngineCell.Application.Views.Pid
{
    /// <summary>
    /// Interaction logic for PidModeSelection.xaml
    /// </summary>
    public partial class PidModeSelection : Window
    {
        private PidDisplayModel LeftPidDisplay { get; set; }
        private PidDisplayModel RightPidDisplay { get; set; }

        public PidModeSelection(PidDisplayModel leftPidDisplay, PidDisplayModel rightPidDisplay)
        {
            InitializeComponent();
            LeftPidDisplay = leftPidDisplay;
            RightPidDisplay = rightPidDisplay;
            SingleMode.Visibility = rightPidDisplay == null ? Visibility.Visible : Visibility.Hidden;
            MultiMode.Visibility = rightPidDisplay == null ? Visibility.Hidden : Visibility.Visible;
            SetupRadioButtons();
            CompareModes();
        }
        
        private void SetupRadioButtons()
        {
            if (RightPidDisplay == null)
            {
                SingleRPM.IsChecked = SingleNm.IsChecked = SinglePercent.IsChecked = false;

                switch (LeftPidDisplay.SettingManual)
                {
                    case ControlConstants.PidSetting.Speed:
                        SingleRPM.IsChecked = true;
                        break;
                    case ControlConstants.PidSetting.Torque:
                        SingleNm.IsChecked = true;
                        break;
                    case ControlConstants.PidSetting.Direct:
                        SinglePercent.IsChecked = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return;
            }

            LeftRpm.IsChecked = LeftNm.IsChecked = LeftPercent.IsChecked = false;
            
            switch (LeftPidDisplay.SettingManual)
            {
                case ControlConstants.PidSetting.Speed:
                    LeftRpm.IsChecked = true;
                    break;
                case ControlConstants.PidSetting.Torque:
                    LeftNm.IsChecked = true;
                    break;
                case ControlConstants.PidSetting.Direct:
                    LeftPercent.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            RightRpm.IsChecked = RightNm.IsChecked = RightPercent.IsChecked = false;
            switch (RightPidDisplay.SettingManual)
            {
                case ControlConstants.PidSetting.Speed:
                    RightRpm.IsChecked = true;
                    break;
                case ControlConstants.PidSetting.Torque:
                    RightNm.IsChecked = true;
                    break;
                case ControlConstants.PidSetting.Direct:
                    RightPercent.IsChecked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CompareModes()
        {
            LeftRpm.IsEnabled = LeftNm.IsEnabled = LeftPercent.IsEnabled = true;
            RightRpm.IsEnabled = RightNm.IsEnabled = RightPercent.IsEnabled = true;
            if (LeftRpm.IsChecked.Value)
                RightRpm.IsEnabled = false;
            if (LeftNm.IsChecked.Value)
                RightNm.IsEnabled = false;
            if (LeftPercent.IsChecked.Value)
                RightPercent.IsEnabled = false;

            if (RightRpm.IsChecked.Value)
                LeftRpm.IsEnabled = false;
            if (RightNm.IsChecked.Value)
                LeftNm.IsEnabled = false;
            if (RightPercent.IsChecked.Value)
                LeftPercent.IsEnabled = false;
        }

        private void RadioButton_OnChecked(object sender, RoutedEventArgs e)
        {
            CompareModes();
        }

        private void SubmitModes(object sender, RoutedEventArgs e)
        {
            if (RightPidDisplay == null)
            {
                if (SingleRPM.IsChecked.Value)
                    LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Speed;
                if (SingleNm.IsChecked.Value)
                    LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Torque;
                if (SinglePercent.IsChecked.Value)
                    LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Direct;
                this.Close();
            }

            if (LeftRpm.IsChecked.Value)
                LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Speed;
            if (LeftNm.IsChecked.Value)
                LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Torque;
            if (LeftPercent.IsChecked.Value)
                LeftPidDisplay.SettingManual = ControlConstants.PidSetting.Direct;
            
            if (RightRpm.IsChecked.Value)
                RightPidDisplay.SettingManual = ControlConstants.PidSetting.Speed;
            if (RightNm.IsChecked.Value)
                RightPidDisplay.SettingManual = ControlConstants.PidSetting.Torque;
            if (RightPercent.IsChecked.Value)
                RightPidDisplay.SettingManual = ControlConstants.PidSetting.Direct;
            
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
