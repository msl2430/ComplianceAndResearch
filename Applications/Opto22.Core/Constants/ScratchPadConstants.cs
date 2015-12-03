
namespace Opto22.Core.Constants
{
    public static class ScratchPadConstants
    {
        public enum BitIndexes
        {
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum IntegerIndexes
        {
            ConnectedToOpto,
            StrategyLocationValue,
            GoToLearningPhase,
            GoToTestPhase,
            GoToSimulator,

            ManufacturerId = 50,
            ModelId,
            ModelYear,
            
            LearningPhaseActive = 100,
            LearingPhaseDataLoadStatus,

            TestPhaseActive = 200,
            TestPhaseDataLoadStatus,        
            
            SimulatorActive = 300,
            SimulatorOverrideType,
            SimulatorDataLoadStatus
        }

        public enum StringIndexes
        {
        }

        public enum StrategyLocationValues
        {
            Off,
            AppRunningCheck,
            PhaseCheck,
            LearningPhase,
            LearningPhaseCalculating,
            TestPhase,
            Simulator,
        }

        public enum LoadStatus
        {
            Off,
            Waiting,
            Loading,
            LoadFinished
        }

        public enum SimulatorOverrides
        {
            Off,
            RequiredSpeed,
            ThrottleSetPoint,
            Acceleration,
            Exit,
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum FloatIndexes
        {
            SpeedPoints = 10,
            AcclerationPoints = 30,

            TestPhaseCurrentSpeed = 200,
            TestPhaseCurrentAcceleration,
            TestPhaseRequiredSpeed,
            TestPhaseThrottleSetPoint,
            TestPhaseCurrentRunTime,
            ModelPulseMultplier = 210,
            ModelPulsePerRev,
            
            GearRatio1 = 250,
            GearRatio2,
            GearRatio3,
            GearRatio4,
            GearRatio5,
            GearRatio6,
            GearRatio7,
            GearRatio8,
            GearRatio9,
            GearRatio10,

            SimulatorRequiredSpeed = 300,
            SimulatorThrottleSetPoint,
            SimulatorAcceleration,
            SimulatorMaxSpeed,
            SimulatorCurrentSpeed,
            SimulatorCurrentAcceleration,
            SimulatorCurrentGear,
            SimulatorCurrentSetPoint,

            TspGear1Start = 1000,
            TspGear2Start = 1100,
            TspGear3Start = 1200,
            TspGear4Start = 1300,
            TspGear5Start = 1400,
            TspGear6Start = 1500,
            TspGear7Start = 1600,
            TspGear8Start = 1700,
            TspGear9Start = 1800,
            TspGear10Start = 1900,
            RoadTestSpeedPerSecond = 2000,
        }
    }
}
