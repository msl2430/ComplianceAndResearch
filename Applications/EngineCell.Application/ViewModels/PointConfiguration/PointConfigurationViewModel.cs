using System;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using Remotion.Linq.Collections;

namespace EngineCell.Application.ViewModels.PointConfiguration
{
    public class PointConfigurationViewModel : BaseViewModel
    {
        private ObservableCollection<PointDataModel> _thermoCouples { get; set; }
        public ObservableCollection<PointDataModel> ThermoCouples
        {
            get { return _thermoCouples; }
            set
            {
                _thermoCouples = value;
                OnPropertyChanged("ThermoCouples");
                OnPropertyChanged("HasThermoCouples");
            }
        }
        public bool HasThermoCouples => ThermoCouples.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _volts { get; set; }
        public ObservableCollection<PointDataModel> Volts
        {
            get { return _volts; }
            set
            {
                _volts = value;
                OnPropertyChanged("Volts");
                OnPropertyChanged("HasVolts");
                OnPropertyChanged("HasVoltsMilliAmpsHarts");
            }
        }
        public bool HasVolts => Volts.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _milliAmps { get; set; }
        public ObservableCollection<PointDataModel> MilliAmps
        {
            get { return _milliAmps; }
            set
            {
                _milliAmps = value;
                OnPropertyChanged("MilliAmps");
                OnPropertyChanged("HasMilliAmps");
                OnPropertyChanged("HasVoltsMilliAmpsHarts");
            }
        }
        public bool HasMilliAmps => MilliAmps.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _harts { get; set; }
        public ObservableCollection<PointDataModel> Harts
        {
            get { return _harts; }
            set
            {
                _harts = value;
                OnPropertyChanged("Harts");
                OnPropertyChanged("HasHarts");
                OnPropertyChanged("HasVoltsMilliAmpsHarts");
            }
        }
        public bool HasHarts => Harts.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _analogOutputs { get; set; }
        public ObservableCollection<PointDataModel> AnalogOutputs
        {
            get { return _analogOutputs; }
            set
            {
                _analogOutputs = value;
                OnPropertyChanged("AnalogOutputs");
                OnPropertyChanged("HasAnalogOutputs");
            }
        }
        public bool HasAnalogOutputs => AnalogOutputs.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _digitalOuts { get; set; }
        public ObservableCollection<PointDataModel> DigitalOuts
        {
            get { return _digitalOuts; }
            set
            {
                _digitalOuts = value;
                OnPropertyChanged("DigitalOuts");
                OnPropertyChanged("HasDigitalOuts");
            }
        }
        public bool HasDigitalOuts => DigitalOuts.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _freqs { get; set; }
        public ObservableCollection<PointDataModel> Freqs
        {
            get { return _freqs; }
            set
            {
                _freqs = value;
                OnPropertyChanged("Freqs");
                OnPropertyChanged("HasFreqs");
            }
        }
        public bool HasFreqs => Freqs.IsNotNullOrEmpty();

        private ObservableCollection<PointDataModel> _digitalIns { get; set; }
        public ObservableCollection<PointDataModel> DigitalIns
        {
            get { return _digitalIns; }
            set
            {
                _digitalIns = value;
                OnPropertyChanged("DigitalIns");
                OnPropertyChanged("HasDigitalIns");
            }
        }
        public bool HasDigitalIns => DigitalIns.IsNotNullOrEmpty();

        public bool HasVoltsMilliAmpsHarts => Volts.IsNotNullOrEmpty() || MilliAmps.IsNotNullOrEmpty() || Harts.IsNotNullOrEmpty();

        public PointConfigurationViewModel(IApplicationSessionFactory appSession)
        {
            ZIndex = 0;
            ApplicationSessionFactory = appSession;
            ThermoCouples = new ObservableCollection<PointDataModel>();
            Volts = new ObservableCollection<PointDataModel>();
            MilliAmps = new ObservableCollection<PointDataModel>();
            Harts = new ObservableCollection<PointDataModel>();
            AnalogOutputs = new ObservableCollection<PointDataModel>();
            DigitalOuts = new ObservableCollection<PointDataModel>();           
            Freqs = new ObservableCollection<PointDataModel>();
            DigitalIns = new ObservableCollection<PointDataModel>();

            if (appSession.CellPoints.IsNullOrEmpty())
                return;

            UpdatePoints();
        }

        private void UpdatePoints()
        {
            ThermoCouples.Clear();
            Volts.Clear();
            MilliAmps.Clear();
            Harts.Clear();
            AnalogOutputs.Clear();
            DigitalOuts.Clear();          
            Freqs.Clear();
            DigitalIns.Clear();

            var tempThermoCouples = new ObservableCollection<PointDataModel>();
            var tempVolts  = new ObservableCollection<PointDataModel>();
            var tempMilliAmps = new ObservableCollection<PointDataModel>();
            var tempHarts = new ObservableCollection<PointDataModel>();
            var tempAnalogOutputs = new ObservableCollection<PointDataModel>();
            var tempDigitalOuts = new ObservableCollection<PointDataModel>();          
            var tempFreqs = new ObservableCollection<PointDataModel>();
            var tempDigitalIns = new ObservableCollection<PointDataModel>();

            foreach (var point in ApplicationSessionFactory.CellPoints)
            {
                switch (point.PointGroupId)
                {
                    case PointGroup.ThermoCouple:
                        tempThermoCouples.Add(point);
                        break;
                    case PointGroup.Volt:
                        tempVolts.Add(point);
                        break;
                    case PointGroup.MilliAmp:
                        tempMilliAmps.Add(point);
                        break;
                    case PointGroup.Hart:
                        tempHarts.Add(point);
                        break;
                    case PointGroup.AnalogOutput:
                        tempAnalogOutputs.Add(point);
                        break;
                    case PointGroup.DigitalOutput:
                        tempDigitalOuts.Add(point);                      
                        break;
                    case PointGroup.Frequency:
                        tempFreqs.Add(point);
                        break;
                    case PointGroup.DigitalInput:
                        tempDigitalIns.Add(point);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            ThermoCouples = tempThermoCouples;
            Volts = tempVolts;
            MilliAmps = tempMilliAmps;
            Harts = tempHarts;
            AnalogOutputs = tempAnalogOutputs;
            DigitalOuts = tempDigitalOuts;
            Freqs = tempFreqs;
            DigitalIns = tempDigitalIns;
        }

        public void UpdateViewModel()
        {
            UpdatePoints();
        }
    }
}
