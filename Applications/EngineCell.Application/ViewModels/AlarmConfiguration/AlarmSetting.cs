using EngineCell.Core.Models;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.AlarmConfiguration
{
    public class AlarmSetting : BaseModel
    {
        private bool _isActive { get; set; }
        public bool IsActive {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); }
        }

        public int CellPointAlarmId { get; set; }

        private decimal _lowShutdownWarning { get; set; }
        public decimal LowShutdownWarning {
            get { return _lowShutdownWarning; }
            set { _lowShutdownWarning = value; OnPropertyChanged("LowShutdownWarning"); }
        }

        private decimal _highShutdownWarning { get; set; }
        public decimal HighShutdownWarning
        {
            get { return _highShutdownWarning; }
            set { _highShutdownWarning = value; OnPropertyChanged("HighShutdownWarning"); }
        }

        private decimal _lowShutdownExecute { get; set; }
        public decimal LowShutdownExecute
        {
            get { return _lowShutdownExecute; }
            set { _lowShutdownExecute = value; OnPropertyChanged("LowShutdownExecute"); }
        }

        private decimal _highShutdownExecute { get; set; }
        public decimal HighShutdownExecute
        {
            get { return _highShutdownExecute; }
            set { _highShutdownExecute = value; OnPropertyChanged("HighShutdownExecute"); }
        }

        public AlarmSetting()
        {
            IsActive = false;
        }

        public AlarmSetting(CellPointAlarmModel alarm)
        {
            IsActive = false;
            if (alarm == null || alarm.CellPointAlarmId <= 0)
                return;

            IsActive = true;
            CellPointAlarmId = alarm.CellPointAlarmId;
            LowShutdownWarning = alarm.LowShutdownWarning;
            LowShutdownExecute = alarm.LowShutdownExecute;
            HighShutdownWarning = alarm.HighShutdownWarning;
            HighShutdownExecute = alarm.HighShutdownExecute;
        }

        public CellPointAlarmModel ToCellPointAlarmModel()
        {
            return new CellPointAlarmModel
            {
                CellPointAlarmId = CellPointAlarmId,
                LowShutdownWarning = LowShutdownWarning,
                LowShutdownExecute = LowShutdownExecute,
                HighShutdownWarning = HighShutdownWarning,
                HighShutdownExecute = HighShutdownExecute
            };
        }
    }
}
