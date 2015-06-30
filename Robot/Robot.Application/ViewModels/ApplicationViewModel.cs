using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MahApps.Metro.Controls;
using Robot.Application.Factories;
using Robot.Core.Constants;

namespace Robot.Application.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
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

        public Flyout AddManufacturerFlyout { get; set; }

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

        public void ChangePageViewModel(BaseViewModel viewModel)
        {
            CurrentPageViewModel = viewModel;
        }
    }
}
