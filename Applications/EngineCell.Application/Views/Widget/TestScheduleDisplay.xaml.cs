using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.DataServices;
using EngineCell.Application.Services.WorkerServices.Widget;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for TestScheduleDisplay.xaml
    /// </summary>
    public partial class TestScheduleDisplay : UserControl
    {
        private TestScheduleDisplayViewModel ViewModel { get; set; }

        private TestScheduleWidgetWorkerService TestScheduleWorkerService { get; set; }

        private IWidgetService _widgetService { get; set; }
        private IWidgetService WidgetService => _widgetService ?? (_widgetService = new WidgetService());

        public TestScheduleDisplay(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            InitializeComponent();
            ViewModel = new TestScheduleDisplayViewModel(appSession, widget);
            widget.IsRunning = true;
            TestScheduleWorkerService = new TestScheduleWidgetWorkerService(ViewModel);
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
            Task.Run(() =>
            {
                TestScheduleWorkerService.DoWork();
            });
        }

        private void PrepareWidget()
        {
            if (ViewModel.Widget.Settings.All(s => s.WidgetSettingId != WidgetConstants.WidgetSetting.TestScheduleFile))
            {
                MessageBox.Show("No test schedule file configured!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule.IsNullOrEmpty())
            {
                if (!File.Exists(ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Value))
                {
                    MessageBox.Show("No test schedule file found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Schedule =
                    WidgetService.ReadTestScheduleFromFile(ViewModel.Widget.Settings.First(s => s.WidgetSettingId == WidgetConstants.WidgetSetting.TestScheduleFile).Value);
            }

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
