using System.ComponentModel;

namespace EngineCell.Application.ViewModels
{
    public class PidSetting : INotifyPropertyChanged
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
                    Proportional = 0;
                    Integral = 0;
                    Derivative = 0;
                }
                OnPropertyChanged("IsManual");
            }
        }

        private decimal _proportional { get; set; }

        public decimal Proportional
        {
            get
            {
                return _proportional;
            }
            set
            {
                _proportional = value;
                IsDirty = true;
                OnPropertyChanged("Proportional");
            }
        }

        private decimal _integral { get; set; }

        public decimal Integral
        {
            get
            {
                return _integral;
            }
            set
            {
                _integral = value;
                IsDirty = true;
                OnPropertyChanged("Integral");
            }
        }

        private decimal _derivative { get; set; }

        public decimal Derivative
        {
            get
            {
                return _derivative;
            }
            set
            {
                _derivative = value;
                IsDirty = true;
                OnPropertyChanged("Derivative");
            }
        }

        public string Name { get; set; }

        private bool _isDirty { get; set; }
        public bool IsDirty { get { return _isDirty;} set { _isDirty = value; OnPropertyChanged("IsDirty"); } }

        public PidSetting()
        {
            IsManual = false;
            IsDirty = false;
        }

        public PidSetting(string name)
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
