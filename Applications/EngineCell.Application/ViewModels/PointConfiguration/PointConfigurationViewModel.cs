using System;
using System.Collections.Generic;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;

namespace EngineCell.Application.ViewModels.PointConfiguration
{
    public class PointConfigurationViewModel : BaseViewModel
    {
        private IList<PointDataModel> _thermalCouples { get; set; }
        public IList<PointDataModel> ThermalCouples
        {
            get { return _thermalCouples; }
            set
            {
                _thermalCouples = value;
                OnPropertyChanged("ThermalCouples");
            }
        }

        private IList<PointDataModel> _volts { get; set; }
        public IList<PointDataModel> Volts
        {
            get { return _volts; }
            set
            {
                _volts = value;
                OnPropertyChanged("Volts");
            }
        }

        private IList<PointDataModel> _milliAmps { get; set; }
        public IList<PointDataModel> MilliAmps
        {
            get { return _milliAmps; }
            set
            {
                _milliAmps = value;
                OnPropertyChanged("MilliAmps");
            }
        }

        private IList<PointDataModel> _harts { get; set; }
        public IList<PointDataModel> Harts
        {
            get { return _harts; }
            set
            {
                _harts = value;
                OnPropertyChanged("Harts");
            }
        }

        private IList<PointDataModel> _analogOutputs { get; set; }
        public IList<PointDataModel> AnalogOutputs
        {
            get { return _analogOutputs; }
            set
            {
                _analogOutputs = value;
                OnPropertyChanged("AnalogOutputs");
            }
        }

        private IList<PointDataModel> _dynos { get; set; }
        public IList<PointDataModel> Dynos
        {
            get { return _dynos; }
            set
            {
                _dynos = value;
                OnPropertyChanged("Dynos");
            }
        }

        private IList<PointDataModel> _engines { get; set; }
        public IList<PointDataModel> Engines
        {
            get { return _engines; }
            set
            {
                _engines = value;
                OnPropertyChanged("Engines");
            }
        }

        private IList<PointDataModel> _fuels { get; set; }
        public IList<PointDataModel> Fuels
        {
            get { return _fuels; }
            set
            {
                _fuels = value;
                OnPropertyChanged("Fuels");
            }
        }

        private IList<PointDataModel> _miscs { get; set; }
        public IList<PointDataModel> Miscs
        {
            get { return _miscs; }
            set
            {
                _miscs = value;
                OnPropertyChanged("Miscs");
            }
        }

        private IList<PointDataModel> _freqs { get; set; }
        public IList<PointDataModel> Freqs
        {
            get { return _freqs; }
            set
            {
                _freqs = value;
                OnPropertyChanged("Freqs");
            }
        }


        public PointConfigurationViewModel(IApplicationSessionFactory appSession)
        {
            ZIndex = 0;
            ApplicationSessionFactory = appSession;
            ThermalCouples = new List<PointDataModel>();
            Volts = new List<PointDataModel>();
            MilliAmps = new List<PointDataModel>();
            Harts = new List<PointDataModel>();
            AnalogOutputs = new List<PointDataModel>();
            Dynos = new List<PointDataModel>();
            Engines = new List<PointDataModel>();
            Fuels = new List<PointDataModel>();
            Miscs = new List<PointDataModel>();
            Freqs = new List<PointDataModel>();

            foreach (var point in appSession.CellPoints)
            {
                switch (point.PointGroupId)
                {
                    case PointGroup.ThermalCouple:
                        ThermalCouples.Add(point);
                        break;
                    case PointGroup.Volt:
                        Volts.Add(point);
                        break;
                    case PointGroup.MilliAmp:
                        MilliAmps.Add(point);
                        break;
                    case PointGroup.Hart:
                        Harts.Add(point);
                        break;
                    case PointGroup.AnalogOutput:
                        AnalogOutputs.Add(point);
                        break;
                    case PointGroup.Dyno:
                        Dynos.Add(point);
                        break;
                    case PointGroup.Engine:
                        Engines.Add(point);
                        break;
                    case PointGroup.Fuel:
                        Fuels.Add(point);
                        break;
                    case PointGroup.Misc:
                        Miscs.Add(point);
                        break;
                    case PointGroup.Frequency:
                        Freqs.Add(point);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
