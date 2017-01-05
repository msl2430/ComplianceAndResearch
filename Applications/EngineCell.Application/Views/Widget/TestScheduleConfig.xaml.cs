using System;
using System.Collections.Generic;
using System.IO;
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
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.DataObjects;
using EngineCell.Models.Models;
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

        public TestScheduleConfig(CellTestPhaseModel phase)
        {
            InitializeComponent();
            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);
            UpdateWidgetStatus();
        }

        private void UploadFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != true) return;

            var streamReader = new StreamReader(dialog.FileName);
            if (!dialog.FileName.Contains(".csv"))
            {
                FileFormatError.Visibility = Visibility.Visible;
                return;
            }

            FileFormatError.Visibility = Visibility.Collapsed;
            string line;
            var throttleIndex = -1;
            var dynoIndex = -1;
            var setPoints = new List<ScheduleData>();
            while ((line = streamReader.ReadLine()) != null)
            {
                var lineArray = line.Split(new[] {",", "\r\n", "\n"}, StringSplitOptions.None);
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

                if(throttleIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[throttleIndex], WidgetConstants.TestScheduleSetpointType.Throttle));
                if(dynoIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[dynoIndex], WidgetConstants.TestScheduleSetpointType.Dyno));
            }

            if (setPoints.IsNullOrEmpty())
            {
                NoSetpointError.Visibility = Visibility.Visible;
                return;
            }
            NoSetpointError.Visibility = Visibility.Collapsed;

            var widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == WidgetConstants.Widget.TestSchedule);
            if (widget == null)
                return;

            widget.Settings.Add(new WidgetSettingModel() { CellTestPhaseWidgetId = widget.CellTestPhaseWidgetId, WidgetSettingId = WidgetConstants.WidgetSetting.TestScheduleFile, Schedule = setPoints});
            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            if (Widget != null && Widget.Settings.Any(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile && w.Schedule.IsNotNullOrEmpty()))
            {
                WidgetStatus.Foreground = Brushes.Green;
                WidgetStatus.Text = "Configured";
                TestTimeLabel.Visibility = Visibility.Visible;
                TestTimeText.Visibility = Visibility.Visible;
                PidLabel.Visibility = Visibility.Visible;
                PidList.Visibility = Visibility.Visible;
                TestTimeText.Text = Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Max(s => s.TimeIntoStage).ToString("0.##");
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Dyno) ?? false)
                    PidList.Children.Add(new TextBlock() {Text = "Dyno"});
                if (Widget.Settings.FirstOrDefault(w => w.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile)?.Schedule.Any(s => s.SetpointType == WidgetConstants.TestScheduleSetpointType.Throttle) ?? false)
                    PidList.Children.Add(new TextBlock() { Text = "Throttle" });
            }
            else
            {
                WidgetStatus.Foreground = Brushes.OrangeRed;
                WidgetStatus.Text = "Not Configured";
                TestTimeLabel.Visibility = Visibility.Collapsed;
                TestTimeText.Visibility = Visibility.Collapsed;
                TestTimeText.Text = "";
                PidLabel.Visibility = Visibility.Collapsed;
                PidList.Visibility = Visibility.Collapsed;
            }
        }
    }
}
