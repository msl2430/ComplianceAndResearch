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

        private decimal _requiredSpeed { get; set; }
        public decimal RequiredSpeed { get { return _requiredSpeed; } set { _requiredSpeed = value; OnPropertyChanged("RequiredSpeed"); } }

        private decimal _throttleSetPoint { get; set; }
        public decimal ThrottleSetPoint { get { return _throttleSetPoint; } set { _throttleSetPoint = value; OnPropertyChanged("ThrottleSetPoint"); } }

        private decimal _acceleration { get; set; }
        public decimal Acceleration { get { return _acceleration; } set { _acceleration = value; OnPropertyChanged("Acceleration"); } }

        private decimal _maxSpeed { get; set; }
        public decimal MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; OnPropertyChanged("MaxSpeed"); } }

        private decimal _rollDiameter { get; set; }
        public decimal RollDiameter { get { return _rollDiameter; } set { _rollDiameter = value; OnPropertyChanged("RollDiameter"); } }

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
