using Opto22.Core.Constants;

namespace Opto22.Core.Models
{
    public class AnalogPointModel : BaseModel
    {
        public int PointNumber { get; set; }
        public Opto22Constants.ModuleType ModuleType { get; set; }
        public Opto22Constants.PointType PointType { get; set; }
        public float HighScale { get; set; }
        public float LowScale { get; set; }
        public float Offset { get; set; }
        public float Gain { get; set; }
        public float FilterWeight { get; set; }
        public bool EnableWatchdog { get; set; }
        public float WatchdogValue { get; set; }
        public string PointName { get; set; }
        public float LowerClamp { get; set; }
        public float UpperClamp { get; set; }

        public AnalogPointModel() { }

        public AnalogPointModel(Parameters parameters)
        {
            if (parameters == null)
                return;

            PointNumber = parameters.PointNumber;
            ModuleType = (Opto22Constants.ModuleType)parameters.ModuleType;
            PointType = (Opto22Constants.PointType)parameters.PointType;
            HighScale = parameters.HighScale;
            LowScale = parameters.LowScale;
            Offset = parameters.Offset;
            Gain = parameters.Gain;
            FilterWeight = parameters.FilterWeight;
            EnableWatchdog = parameters.EnableWatchdog;
            WatchdogValue = parameters.WatchdogValue;
            PointName = parameters.PointName;
            LowerClamp = parameters.LowerClamp;
            UpperClamp = parameters.UpperClamp;
        }

        public class Parameters
        {
            public int PointNumber, ModuleType, PointType;
            public float HighScale, LowScale, Offset, Gain, FilterWeight, WatchdogValue, LowerClamp, UpperClamp;
            public bool EnableWatchdog;
            public string PointName;
        }
    }
}
