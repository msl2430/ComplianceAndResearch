
namespace Opto22TestProject
{
    public class DigitalPointConfiguration : BaseModel
    {
        public int PointNumber { get; set; }
        public Constants.ModuleType ModuleType { get; set; }
        public Constants.PointType PointType { get; set; }
        public int DigitalFeature { get; set; }
        public bool EnableWatchdog { get; set; }
        public bool WatchdogValue { get; set; }
        public string PointName { get; set; }

        public DigitalPointConfiguration() { }

        public DigitalPointConfiguration(Parameters parameters)
        {
            if (parameters == null)
                return;

            PointNumber = parameters.PointNumber;
            ModuleType = (Constants.ModuleType)parameters.ModuleType;
            PointType = (Constants.PointType)parameters.PointType;
            EnableWatchdog = parameters.EnableWatchdog;
            WatchdogValue = parameters.WatchdogValue;
            PointName = parameters.PointName;
        }

        public class Parameters
        {
            public int PointNumber, ModuleType, PointType, DigitalFeature;
            public bool EnableWatchdog, WatchdogValue;
            public string PointName;
        }
    }
}
