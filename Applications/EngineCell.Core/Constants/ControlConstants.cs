namespace EngineCell.Core.Constants
{
    public static class ControlConstants
    {
        public static int TextBlockMaxCharacter = 10000;

        public enum ChangeTracking
        {
            Pristine,
            Dirty
        }

        public enum ProgressType
        {
            ProgressRing,
            Idle,
            Finished,
        }

        public enum PidSetting
        {
            Speed,
            Torque,
            Direct
        }

        public enum PidType
        {
            Dyno = 1,
            Throttle,
            Coolant,
            Oil,
            Intercooler,
            Custom1,
            Custom2,
            Custom3,
            Custom4
        }

        public enum CellTestType
        {
            Manual = 1,
            Timed,
        }

        public enum Views
        {
            Welcome,
            TestDisplay,
            PointConfiguration,
            PhaseConfiguration
        }
    }
}
