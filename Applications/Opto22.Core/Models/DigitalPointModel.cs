using Opto22.Core.Constants;

namespace Opto22.Core.Models
{
    public class DigitalPointModel : BaseModel
    {
        public int PointNumber { get; set; }
        public Opto22Constants.ModuleType ModuleType { get; set; }
        public Opto22Constants.PointType PointType { get; set; }
        public int DigitalFeature { get; set; }
        public bool EnableWatchdog { get; set; }
        public bool WatchdogValue { get; set; }
        public string PointName { get; set; }

        public DigitalPointModel() { }

        public DigitalPointModel(Parameters parameters)
        {
            if (parameters == null)
                return;

            PointNumber = parameters.PointNumber;
            ModuleType = (Opto22Constants.ModuleType)parameters.ModuleType;
            PointType = (Opto22Constants.PointType)parameters.PointType;
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
