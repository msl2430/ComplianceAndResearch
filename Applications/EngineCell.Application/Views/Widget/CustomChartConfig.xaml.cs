using System;
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

namespace EngineCell.Application.Views.Widget
{
    /// <summary>
    /// Interaction logic for CustomChartConfig.xaml
    /// </summary>
    public partial class CustomChartConfig : UserControl
    {
        private CellTestPhaseModel Phase { get; set; }
        private CellTestPhaseWidgetModel Widget { get; set; }
        public bool IsConfigured { get; set; }

        private IWidgetRepository _widgetRepository { get; set; }
        private IWidgetRepository WidgetRepository => _widgetRepository ?? (_widgetRepository = new WidgetRepository());

        private IWidgetService _widgetService { get; set; }
        private IWidgetService WidgetService => _widgetService ?? (_widgetService = new WidgetService());

        public CustomChartConfig(CellTestPhaseModel phase, WidgetConstants.Widget type)
        {
            InitializeComponent();
            Phase = phase;
            Widget = Phase.Widgets.FirstOrDefault(w => w.WidgetId == type);

            switch (type)
            {
                case WidgetConstants.Widget.CustomChart1:
                    ChartName.Text = "Custom Chart 1";
                    break;
                case WidgetConstants.Widget.CustomChart2:
                    ChartName.Text = "Custom Chart 2";
                    break;
                case WidgetConstants.Widget.CustomChart3:
                    ChartName.Text = "Custom Chart 3";
                    break;
                case WidgetConstants.Widget.CustomChart4:
                    ChartName.Text = "Custom Chart 4";
                    break;
                case WidgetConstants.Widget.CustomChart5:
                    ChartName.Text = "Custom Chart 5";
                    break;                
            }

            UpdateWidgetStatus();
        }

        private void UpdateWidgetStatus()
        {
            ErrorTimeoutLabel.Visibility = Visibility.Visible;
            ErrorTimeoutText.Visibility = Visibility.Visible;
            ErrorTimeoutBorder.Visibility = Visibility.Visible;
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
