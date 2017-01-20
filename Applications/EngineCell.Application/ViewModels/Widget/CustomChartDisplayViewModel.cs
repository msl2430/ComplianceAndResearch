using System;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Widget
{
    [Serializable]
    public class CustomChartDisplayViewModel : BaseViewModel
    {
        public CellTestPhaseWidgetModel Widget { get; set; }

        private string _currentTime { get; set; }
        public string CurrentTime { get { return _currentTime; } set { _currentTime = value; OnPropertyChanged("CurrentTime"); } }

        public string ChartName { get; }

        public CustomChartDisplayViewModel(IApplicationSessionFactory appSession, CellTestPhaseWidgetModel widget)
        {
            ApplicationSessionFactory = appSession;
            Widget = widget;
            CurrentTime = "00:00:00";
            switch (Widget.WidgetId)
            {
                case WidgetConstants.Widget.CustomChart1:
                    ChartName = "Custom Chart 1";
                    break;
                case WidgetConstants.Widget.CustomChart2:
                    ChartName = "Custom Chart 2";
                    break;
                case WidgetConstants.Widget.CustomChart3:
                    ChartName = "Custom Chart 3";
                    break;
                case WidgetConstants.Widget.CustomChart4:
                    ChartName = "Custom Chart 4";
                    break;
                case WidgetConstants.Widget.CustomChart5:
                    ChartName = "Custom Chart 5";
                    break;                
                default:
                    ChartName = "";
                    break;
            }
        }
    }
}
