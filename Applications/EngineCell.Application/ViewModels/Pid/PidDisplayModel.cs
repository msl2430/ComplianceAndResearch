using EngineCell.Core.Constants;
using EngineCell.Core.Models;

namespace EngineCell.Application.ViewModels.Pid
{
    public class PidDisplayModel : BaseModel
    {
        public string PidName { get; set; }

        private bool _isAuto { get; set; }
        public bool IsAuto {
            get { return _isAuto; }
            set {
                _isAuto = value;
                OnPropertyChanged("IsAuto");
                OnPropertyChanged("IsManual");
                OnPropertyChanged("SettingManual");
                OnPropertyChanged("IsSpeedManual");
                OnPropertyChanged("IsTorqueManual");
                OnPropertyChanged("IsDirectManual");
                OnPropertyChanged("IsSpeedManualEnabled");
                OnPropertyChanged("IsTorqueManualEnabled");
                OnPropertyChanged("IsDirectManualEnabled");
            }
        }

        private decimal _autoValue { get; set; }
        public decimal AutoValue {
            get { return _autoValue; } 
            set { _autoValue = value; OnPropertyChanged("AutoValue"); }
        }

        public bool IsManual => !_isAuto;
        
        private decimal _speed { get; set; }
        public decimal Speed {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged("Speed"); }
        }

        private decimal _torque { get; set; }

        public decimal Torque
        {
            get { return _torque; }
            set { _torque = value; OnPropertyChanged("Torque"); }
        }

        private decimal _direct { get; set; }
        public decimal Direct {
            get { return _direct; }
            set { _direct = value; OnPropertyChanged("Direct"); }
        }

        private ControlConstants.PidSetting _settingManual { get;set; }
        public ControlConstants.PidSetting SettingManual {
            get { return _settingManual; }
            set {
                _settingManual = value;
                OnPropertyChanged("SettingManual");
                OnPropertyChanged("IsSpeedManual");
                OnPropertyChanged("IsTorqueManual");
                OnPropertyChanged("IsDirectManual");
                OnPropertyChanged("IsSpeedManualEnabled");
                OnPropertyChanged("IsTorqueManualEnabled");
                OnPropertyChanged("IsDirectManualEnabled");
            }
        }

        private ControlConstants.PidSetting _settingAuto { get; set; }
        public ControlConstants.PidSetting SettingAuto
        {
            get { return _settingAuto; }
            set
            {
                _settingAuto = value;
                OnPropertyChanged("SettingAuto");
                OnPropertyChanged("IsSpeedAuto");
                OnPropertyChanged("IsTorqueAuto");
                OnPropertyChanged("IsDirectAuto");
                OnPropertyChanged("IsSpeedAutoEnabled");
                OnPropertyChanged("IsTorqueAutoEnabled");
                OnPropertyChanged("IsDirectAutoEnabled");
            }
        }

        public bool IsSpeedAuto => SettingAuto == ControlConstants.PidSetting.Speed;
        public bool IsTorqueAuto => SettingAuto == ControlConstants.PidSetting.Torque;
        public bool IsDirectAuto => SettingAuto == ControlConstants.PidSetting.Direct;

        public bool IsSpeedManual => SettingManual == ControlConstants.PidSetting.Speed;
        public bool IsTorqueManual => SettingManual == ControlConstants.PidSetting.Torque;
        public bool IsDirectManual => SettingManual == ControlConstants.PidSetting.Direct;

        public bool IsSpeedManualEnabled => IsSpeedManual && IsManual && SettingManual == ControlConstants.PidSetting.Speed;
        public bool IsTorqueManualEnabled => IsTorqueManual && IsManual && SettingManual == ControlConstants.PidSetting.Torque;
        public bool IsDirectManualEnabled => IsDirectManual && IsManual && SettingManual == ControlConstants.PidSetting.Direct;

        public PidDisplayModel(string name)
        {
            PidName = name;
            SettingAuto = ControlConstants.PidSetting.Speed;
            SettingManual = ControlConstants.PidSetting.Torque;
            IsAuto = true;
        }
    }
}
