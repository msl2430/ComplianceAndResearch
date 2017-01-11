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
        public bool HasThermoCouples { get { return ThermoCouples.IsNotNullOrEmpty(); } }

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
        public bool HasVolts { get { return Volts.IsNotNullOrEmpty(); } }

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
        public bool HasMilliAmps { get { return MilliAmps.IsNotNullOrEmpty(); } }

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
        public bool HasHarts { get { return Harts.IsNotNullOrEmpty(); } }

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
        public bool HasAnalogOutputs { get { return AnalogOutputs.IsNotNullOrEmpty(); } }

        private ObservableCollection<PointDataModel> _dynos { get; set; }
        public ObservableCollection<PointDataModel> Dynos
        {
            get { return _dynos; }
            set
            {
                _dynos = value;
                OnPropertyChanged("Dynos");
                OnPropertyChanged("HasDynos");
            }
        }
        public bool HasDynos { get { return Dynos.IsNotNullOrEmpty(); } }

        private ObservableCollection<PointDataModel> _engines { get; set; }
        public ObservableCollection<PointDataModel> Engines
        {
            get { return _engines; }
            set
            {
                _engines = value;
                OnPropertyChanged("Engines");
                OnPropertyChanged("HasEngines");
            }
        }
        public bool HasEngines { get { return Engines.IsNotNullOrEmpty(); } }

        private ObservableCollection<PointDataModel> _fuels { get; set; }
        public ObservableCollection<PointDataModel> Fuels
        {
            get { return _fuels; }
            set
            {
                _fuels = value;
                OnPropertyChanged("Fuels");
                OnPropertyChanged("HasFuels");
            }
        }
        public bool HasFuels { get { return Fuels.IsNotNullOrEmpty(); } }

        private ObservableCollection<PointDataModel> _miscs { get; set; }
        public ObservableCollection<PointDataModel> Miscs
        {
            get { return _miscs; }
            set
            {
                _miscs = value;
                OnPropertyChanged("Miscs");
                OnPropertyChanged("HasMiscs");
            }
        }
        public bool HasMiscs { get { return Miscs.IsNotNullOrEmpty(); } }

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
        public bool HasFreqs { get { return Freqs.IsNotNullOrEmpty(); } }

        public bool HasVoltsMilliAmpsHarts { get { return Volts.IsNotNullOrEmpty() || MilliAmps.IsNotNullOrEmpty() || Harts.IsNotNullOrEmpty(); } }

        public PointConfigurationViewModel(IApplicationSessionFactory appSession)
        {
            ZIndex = 0;
            ApplicationSessionFactory = appSession;
            ThermoCouples = new ObservableCollection<PointDataModel>();
            Volts = new ObservableCollection<PointDataModel>();
            MilliAmps = new ObservableCollection<PointDataModel>();
            Harts = new ObservableCollection<PointDataModel>();
            AnalogOutputs = new ObservableCollection<PointDataModel>();
            Dynos = new ObservableCollection<PointDataModel>();
            Engines = new ObservableCollection<PointDataModel>();
            Fuels = new ObservableCollection<PointDataModel>();
            Miscs = new ObservableCollection<PointDataModel>();
            Freqs = new ObservableCollection<PointDataModel>();

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
            Dynos.Clear();
            Engines.Clear();
            Fuels.Clear();
            Miscs.Clear();
            Freqs.Clear();

            var tempThermoCouples = new ObservableCollection<PointDataModel>();
            var tempVolts  = new ObservableCollection<PointDataModel>();
            var tempMilliAmps = new ObservableCollection<PointDataModel>();
            var tempHarts = new ObservableCollection<PointDataModel>();
            var tempAnalogOutputs = new ObservableCollection<PointDataModel>();
            var tempDynos = new ObservableCollection<PointDataModel>();
            var tempEngines = new ObservableCollection<PointDataModel>();
            var tempFuels = new ObservableCollection<PointDataModel>();
            var tempMiscs = new ObservableCollection<PointDataModel>();
            var tempFreqs = new ObservableCollection<PointDataModel>();
            
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
                    case PointGroup.Dyno:
                        tempDynos.Add(point);
                        break;
                    case PointGroup.Engine:
                        tempEngines.Add(point);
                        break;
                    case PointGroup.Fuel:
                        tempFuels.Add(point);
                        break;
                    case PointGroup.Misc:
                        tempMiscs.Add(point);
                        break;
                    case PointGroup.Frequency:
                        tempFreqs.Add(point);
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
            Dynos = tempDynos;
            Engines = tempEngines;
            Fuels = tempFuels;
            Miscs = tempMiscs;
            Freqs = tempFreqs;
        }

        public void CellSelectionChange()
        {
            UpdatePoints();
        }
    }
}
