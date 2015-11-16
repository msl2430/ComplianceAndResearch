using Robot.Application.Factories;
using Robot.Models.Models;
using System.Collections.Generic;
using Opto22.Core.Constants;

namespace Robot.Application.ViewModels
{
    public class SimulatorViewModel : BaseViewModel
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string ConfigurationHeader { get; set; }

        private decimal _pulseMultiplier { get; set; }
        public decimal PulseMultiplier { get { return _pulseMultiplier; } set { _pulseMultiplier = value; OnPropertyChanged("PulseMultiplier"); } }

        private decimal _pulsePerRev { get; set; }
        public decimal PulsePerRev { get {return _pulsePerRev;} set { _pulsePerRev = value; OnPropertyChanged("PulsePerRev"); } }

        private ScratchPadConstants.SimulatorOverrides _overrideEnabled { get; set; }
        public ScratchPadConstants.SimulatorOverrides OverrideEnabled { get { return _overrideEnabled; } set { _overrideEnabled = value; OnPropertyChanged("OverrideEnabled"); } }

        public IList<GearRatioModel> ModelGearRatios { get; set; }

        public string CarDisplayName { get { return string.Format("{0} {1} {2}", ModelYear, ManufacturerName, ModelName); } }

        public SimulatorViewModel(IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
        }
    }
}
