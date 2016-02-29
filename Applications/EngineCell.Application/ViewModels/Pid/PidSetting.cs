using System;
using System.ComponentModel;

namespace EngineCell.Application.ViewModels.Pid
{
    [Obsolete]
    public class PidSettingOld : INotifyPropertyChanged
    {
        private bool _isManual { get; set; }

        public bool IsManual
        {
            get { return _isManual; }
            set
            {
                _isManual = value;
                if (!value)
                {
                    Output = 0;
                }
                OnPropertyChanged("IsManual");
            }
        }

        private decimal _output { get; set; }

        public decimal Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                IsDirty = false;
                OnPropertyChanged("Output");
            }
        }
        
        public string Name { get; set; }

        private bool _isDirty { get; set; }
        public bool IsDirty { get { return _isDirty;} set { _isDirty = value; OnPropertyChanged("IsDirty"); } }

        public PidSettingOld()
        {
            IsManual = false;
            IsDirty = false;
        }

        public PidSettingOld(string name)
        {
            Name = name;
            IsManual = false;
            IsDirty = false;
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
