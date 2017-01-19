﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EngineCell.Application.Services.DataServices;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;
using EngineCell.Models.Repositories;
using Microsoft.Win32;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for TestSchedule.xaml
    /// </summary>
    public partial class TestScheduleConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }
        public bool IsConfigured { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        private IWidgetService _widgetService { get; set; }
        private IWidgetService WidgetService => _widgetService ?? (_widgetService = new WidgetService());

        public TestScheduleConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();
            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);

            if (Widget != null && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile))
            {
                ReadFile(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Value, false);
                if (Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleTimeout))
                {
                    TestTimeoutText.Text = Convert.ToInt32(Widget.Settings.FirstOrDefault(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleTimeout).Value).ToString();
                }
                else
                {
                    TestTimeoutText.Text = Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule.Max(s => s.TimeIntoStage).ToString();
                    SaveTestTimeoutSetting(Convert.ToInt32(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule.Max(s => s.TimeIntoStage)));
                }
            }



            UpdateWidgetStatus();
        }

        private void UploadFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != true) return;

            if (!dialog.FileName.Contains(".csv"))
            {
                FileFormatError.Visibility = Visibility.Visible;
                return;
            }

            FileFormatError.Visibility = Visibility.Hidden;
            ReadFile(dialog.FileName, true);
        }

        private void ReadFile(string fileName, bool isNewFile)
        {
            var setPoints = WidgetService.ReadTestScheduleFromFile(fileName);
            
            if (setPoints.IsNullOrEmpty())
            {
                NoSetpointError.Visibility = Visibility.Visible;
                return;
            }
            NoSetpointError.Visibility = Visibility.Hidden;

            var widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);
            if (widget == null)
                return;

            var setting = new WidgetSettingModel() { CellTestPhaseWidgetId = widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.TestScheduleFile, Schedule = setPoints, Value = fileName };
            if (widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile))
            {
                widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule = setting.Schedule;
                if(isNewFile) 
                    WidgetRepository.UpdateWidgetSetting(widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).CellTestPhaseWidgetSettingId, setting.Value);
            }
            else
            {
                widget.Settings.Add(setting);
                WidgetRepository.SaveWidgetSetting(widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.TestScheduleFile, setting.Value);
            }
            TestTimeoutText.Text = Convert.ToInt32(setting.Schedule.Max(s => s.TimeIntoStage)).ToString();
            SaveTestTimeoutSetting(Convert.ToInt32(setting.Schedule.Max(s => s.TimeIntoStage)));
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            PidList.Children.Clear();
            if (Widget != null && Widget.Settings.Any(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile && w.Schedule.IsNotNullOrEmpty()) && Widget.Settings.Any(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleTimeout))
            {
                WidgetStatus.Foreground = (Brush) FindResource("GreenBrush");
                WidgetStatus.Text = "Configured";
                TestTimeLabel.Visibility = Visibility.Visible;
                TestTimeText.Visibility = Visibility.Visible;
                PidLabel.Visibility = Visibility.Visible;
                PidList.Visibility = Visibility.Visible;
                TestTimeText.Text = Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Max(s => s.TimeIntoStage).ToString("0.##");
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Dyno) ?? false)
                    PidList.Children.Add(new TextBlock() {Text = "Dyno", Foreground = Brushes.WhiteSmoke});
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Throttle) ?? false)
                    PidList.Children.Add(new TextBlock() { Text = "Throttle", Foreground = Brushes.WhiteSmoke });                
            }
            else
            {
                WidgetStatus.Foreground = Brushes.OrangeRed;
                WidgetStatus.Text = "Not Configured";
                TestTimeLabel.Visibility = Visibility.Hidden;
                TestTimeText.Visibility = Visibility.Hidden;
                TestTimeText.Text = "";
                PidLabel.Visibility = Visibility.Hidden;
                PidList.Visibility = Visibility.Hidden;                
            }
        }

        private void TestTimeoutText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox) sender).Text))
                return;
            SaveTestTimeoutSetting(Convert.ToInt32(((TextBox) sender).Text));
        }

        private void SaveTestTimeoutSetting(int timeout)
        {
            var widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);
            var setting = new WidgetSettingModel() { CellTestPhaseWidgetId = widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.TestScheduleTimeout, Value = timeout.ToString() };
            widget.Settings.Add(setting);
            WidgetRepository.SaveWidgetSetting(widget.CellTestPhaseWidgetId, WidgetConstants.WidgetSetting.TestScheduleTimeout, setting.Value);
        }
    }
}
