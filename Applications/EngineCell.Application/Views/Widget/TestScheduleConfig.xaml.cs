using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public TestScheduleConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();
            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);

            if (Widget != null && Widget.Settings.Any(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile))
                ReadFile(Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Value, false);

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
            var streamReader = new StreamReader(fileName);

            string line;
            var throttleIndex = -1;
            var dynoIndex = -1;
            var setPoints = new List<ScheduleData>();
            while ((line = streamReader.ReadLine()) != null)
            {
                var lineArray = line.Split(new[] { ",", "\r\n", "\n" }, StringSplitOptions.None);
                if (lineArray[0].ToLower() == "note") //line to find out which setpoint is in which index
                {
                    for (var i = 0; i < lineArray.Length; i++)
                    {
                        if (lineArray[i].ToLower() == "throt" && setPoints.IsNullOrEmpty())
                            throttleIndex = i;
                        if (lineArray[i].ToLower() == "dyno" && setPoints.IsNullOrEmpty())
                            dynoIndex = i;
                    }
                }

                if (throttleIndex < 0 || dynoIndex < 0) //if we haven't set the index of our setpoints then we bail out 
                    continue;

                if (lineArray[0].ToLower() != "data") continue;

                if (throttleIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[throttleIndex], WidgetConstants.TestScheduleSetpointType.Throttle));
                if (dynoIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[dynoIndex], WidgetConstants.TestScheduleSetpointType.Dyno));
            }

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

            var phaseEndSetting = new PhaseEndSettingModel() { CellTestPhaseWidgetId = widget.CellTestPhaseWidgetId, PhaseEndSettingId = WidgetConstants.PhaseEndSetting.RunTime, Value = setPoints.Max(s => s.TimeIntoStage).ToString("0.##") };
            if (isNewFile && widget.PhaseEndSettings.Any(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime))
            {
                widget.PhaseEndSettings.First(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime).Value = phaseEndSetting.Value;
                WidgetRepository.UpdateWidgetPhaseEndSetting(widget.PhaseEndSettings.First(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime).CellTestPhaseWidgetPhaseEndSettingId, phaseEndSetting.Value);
            }
            else
            {                
                widget.PhaseEndSettings.Add(phaseEndSetting);
                WidgetRepository.SavePhaseEndSetting(widget.CellTestPhaseWidgetId, WidgetConstants.PhaseEndSetting.RunTime, phaseEndSetting.Value);
            }
            
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget != null && Widget.Settings.Any(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile && w.Schedule.IsNotNullOrEmpty()))
            {
                WidgetStatus.Foreground = (Brush) FindResource("GreenBrush");
                WidgetStatus.Text = "Configured";
                TestTimeLabel.Visibility = Visibility.Visible;
                TestTimeText.Visibility = Visibility.Visible;
                PidLabel.Visibility = Visibility.Visible;
                PidList.Visibility = Visibility.Visible;
                ErrorTimeoutLabel.Visibility = Visibility.Visible;
                ErrorTimeoutText.Visibility = Visibility.Visible;
                ErrorTimeoutBorder.Visibility = Visibility.Visible;
                TestTimeText.Text = Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Max(s => s.TimeIntoStage).ToString("0.##");
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Dyno) ?? false)
                    PidList.Children.Add(new TextBlock() {Text = "Dyno"});
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Throttle) ?? false)
                    PidList.Children.Add(new TextBlock() { Text = "Throttle" });
                if (Widget.PhaseEndSettings.Any(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime))
                    ErrorTimeoutText.Text = Widget.PhaseEndSettings.First(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime).Value;
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
                ErrorTimeoutLabel.Visibility = Visibility.Hidden;
                ErrorTimeoutText.Visibility = Visibility.Hidden;
                ErrorTimeoutBorder.Visibility = Visibility.Hidden;
            }
        }

        private void ErrorTimeoutText_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);
            var newValue = ((TextBox) sender).Text;

            if (widget.PhaseEndSettings.All(s => s.PhaseEndSettingId != WidgetConstants.PhaseEndSetting.RunTime))
                return;
            
            widget.PhaseEndSettings.First(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime).Value = newValue;
            WidgetRepository.UpdateWidgetPhaseEndSetting(widget.PhaseEndSettings.First(s => s.PhaseEndSettingId == WidgetConstants.PhaseEndSetting.RunTime).CellTestPhaseWidgetPhaseEndSettingId, newValue);
        }
    }
}
