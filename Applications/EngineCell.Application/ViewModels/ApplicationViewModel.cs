using System;
using System.Collections.Generic;
using EngineCell.Core.Constants;

namespace EngineCell.Application.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        public PidSetting CoolantPid { get; set; }

        public PidSetting ThrottlePid { get; set; }

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

        private BaseViewModel _currentPageViewModel { get; set; }
        public BaseViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private string _titleLabel { get; set; }
        public string TitleLabel
        {
            get { return _titleLabel; }
            set { _titleLabel = value; OnPropertyChanged("TitleLabel"); }
        }

        public IList<BaseViewModel> PageViewModelNavigationPath { get; set; }

        public ApplicationViewModel()
        {
            PageViewModelNavigationPath = new List<BaseViewModel>();
        }

        public void ChangePageViewModel(BaseViewModel viewModel)
        {
            if (CurrentPageViewModel != null)
                PageViewModelNavigationPath.Add(CurrentPageViewModel);
            CurrentPageViewModel = viewModel;
        }
    }
}
