using System;
using System.Collections.Generic;
using EngineCell.Application.ViewModels.Phase;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Application.ViewModels.Welcome;
using EngineCell.Application.ViewModels.Widget;
using EngineCell.Application.Views.Widget;
using EngineCell.Core.Constants;
using Remotion.Linq.Collections;

namespace EngineCell.Application.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _logWindowString { get; set; }
        public string LogWindowString
        {
            get { return _logWindowString; }
            set
            {
                _logWindowString = DateTime.Now.ToLongTimeString() + " >> " + value + "\n" + _logWindowString;
                if (_logWindowString.Length > ControlConstants.TextBlockMaxCharacter)
                    _logWindowString.Remove(ControlConstants.TextBlockMaxCharacter);
                OnPropertyChanged("LogWindowString");
            }
        }

        private string _statusLabel { get; set; }
        public string StatusLabel
        {
            get { return _statusLabel; }
            set
            {
                _statusLabel = value;
                OnPropertyChanged("StatusLabel");
            }
        }

        private StatusConstants.ConnectionStatus _optoConnectionStatus { get; set; }
        public StatusConstants.ConnectionStatus OptoConnectionStatus
        {
            get { return _optoConnectionStatus; }
            set
            {
                _optoConnectionStatus = value;
                switch (_optoConnectionStatus)
                {
                    case StatusConstants.ConnectionStatus.Disconnected:
                    case StatusConstants.ConnectionStatus.Disconnecting:
                    case StatusConstants.ConnectionStatus.Connecting:
                        IsConnected = false;
                        break;
                    case StatusConstants.ConnectionStatus.Connected:
                        IsConnected = true;
                        break;
                }
                OnPropertyChanged("OptoConnectionStatus");
            }
        }

        private bool _isConnected { get; set; }
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        private ObservableCollection<BaseViewModel> _viewModels { get; set; }

        public ObservableCollection<BaseViewModel> ViewModels
        {
            get { return _viewModels;}
            set { _viewModels = value; OnPropertyChanged("ViewModels"); }
        } 

        private WelcomeScreenViewModel _welcomeViewModel { get; set; }
        public WelcomeScreenViewModel WelcomeViewModel
        {
            get { return _welcomeViewModel;}
            set { _welcomeViewModel = value; OnPropertyChanged("WelcomeViewModel"); }
        }

        private PointConfigurationViewModel _pointConfigViewModel { get; set; }
        public PointConfigurationViewModel PointConfigViewModel {
            get { return _pointConfigViewModel; }
            set { _pointConfigViewModel = value; OnPropertyChanged("PointConfigViewModel"); }
        }

        private TestDisplayViewModel _testDisplayViewModel { get; set; }
        public TestDisplayViewModel TestDisplayViewModel
        {
            get { return _testDisplayViewModel; }
            set { _testDisplayViewModel = value; OnPropertyChanged("TestDisplayViewModel"); }
        }

        //private WidgetConfigViewModel _widgetConfigViewModel { get; set; }
        //public WidgetConfigViewModel WidgetConfigViewModel {
        //    get { return _widgetConfigViewModel; }
        //    set { _widgetConfigViewModel = value; OnPropertyChanged("WidgetConfigViewModel"); }
        //}

        private PhaseConfigViewModel _phaseConfigViewModel { get; set; }
        public PhaseConfigViewModel PhaseConfigViewModel
        {
            get { return _phaseConfigViewModel; }
            set { _phaseConfigViewModel = value; OnPropertyChanged("PhaseConfigViewModel"); }
        }

        private string _cellTestName { get; set; }
        public string CellTestName
        {
            get { return _cellTestName; }
            set { _cellTestName = value; OnPropertyChanged("CellTestName"); }
        }

        private bool _hasTestActive { get; set; }
        public bool HasTestActive
        {
            get { return _hasTestActive; }
            set { _hasTestActive = value; OnPropertyChanged("HasTestActive"); }
        }

        public IList<BaseViewModel> PageViewModelNavigationPath { get; set; }

        public MainWindowViewModel()
        {
            PageViewModelNavigationPath = new List<BaseViewModel>();            
        }
    }
}
