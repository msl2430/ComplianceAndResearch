
namespace Opto22.Core.Constants
{
    public static class ScratchPadConstants
    {
        public enum BitIndexes
        {
        }

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
            LearingPhaseTemplateLoadStatus =101
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
            TspChart1Start = 1000

        }
    }
}
