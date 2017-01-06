using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for TestScheduleDisplay.xaml
    /// </summary>
    public partial class TestScheduleDisplay : UserControl
    {
        private TestScheduleDisplayViewModel ViewModel { get; set; }

        public TestScheduleDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            ViewModel = new TestScheduleDisplayViewModel(appSession, widget);
            if (ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.TestScheduleFile))
            {
                //TODO: Show Error Screen
                return;
            }
        
        }

        private void TestScheduleDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            PrepareWidget();
        }

        private void PrepareWidget()
        {
            var schedule = ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule;
            ViewModel.CalculatedScheduleData =
                schedule.GroupBy(s => s.SetpointType)
                    .Select(s => s.Key)
                    .Select(setpointType => new KeyValuePair<WidgetConstants.TestScheduleSetpointType, IList<ScheduleData>>(setpointType, schedule.Where(s => s.SetpointType == setpointType).OrderBy(s => s.TimeIntoStage).ToList()))
                    .ToList();

            ViewModel.HasDyno = ViewModel.CalculatedScheduleData.Any(d => d.Key == WidgetConstants.TestScheduleSetpointType.Dyno);
            ViewModel.HasThrottle = ViewModel.CalculatedScheduleData.Any(d => d.Key == WidgetConstants.TestScheduleSetpointType.Throttle);
        }

        
    }
}
