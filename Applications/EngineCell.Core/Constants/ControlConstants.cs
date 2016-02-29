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
    }
}
