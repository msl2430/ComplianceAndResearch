
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

            ManufacturerId = 50,
            ModelId,
            ModelYear,

            LearningPhaseActive = 100,
            LearingPhaseTemplateLoadStatus,

            TestPhaseActive = 200,
            TestPhaseDataLoadStatus,            
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
            LearningPhaseTemplateLoad,
            TestPhase
        }

        public enum LoadStatus
        {
            Off,
            Waiting,
            Loading,
            LoadFinished
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
