
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
            Test1,
            Test2,
            Test3,
            Test4
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
            Waiting,
            Loading,
            LoadFinished
        }
    }
}
