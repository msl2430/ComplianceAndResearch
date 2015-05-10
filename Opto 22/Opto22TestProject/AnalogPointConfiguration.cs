using System;

namespace Opto22TestProject
{
    public class AnalogPointConfiguration : BaseModel
    {
        public int PointNumber { get; set; }
        public Constants.ModuleType ModuleType { get; set; }
        public Constants.PointType PointType { get; set; }
        public float HighScale { get; set; }
        public float LowScale { get; set; }
        public float Offset { get; set; }
        public float Gain { get; set; }
        public float FilterWeight { get; set; }
        public bool EnableWatchdog { get; set; }
        public float WatchdogValue { get; set; }
        public string PointName { get; set; }

        public AnalogPointConfiguration() { }

        public AnalogPointConfiguration(Parameters parameters)
        {
            if (parameters == null)
                return;

            PointNumber = parameters.PointNumber;
            ModuleType = (Constants.ModuleType)parameters.ModuleType;
            PointType = (Constants.PointType)parameters.PointType;
            HighScale = parameters.HighScale;
            LowScale = parameters.LowScale;
            Offset = parameters.Offset;
            Gain = parameters.Gain;
            FilterWeight = parameters.FilterWeight;
            EnableWatchdog = parameters.EnableWatchdog;
            WatchdogValue = parameters.WatchdogValue;
            PointName = parameters.PointName;
        }

        public void WriteConfigToConsole()
        {
            Console.WriteLine("Point " + PointNumber + ": \nModType: " + Enum.GetName(typeof (Constants.ModuleType), ModuleType) + "\nPointType: " +
                              Enum.GetName(typeof (Constants.PointType), PointType) + "\nHighScale: " + HighScale + "\nLowScale: " + LowScale +
                              "\nOffset: " + Offset + "\nGain: :" + Gain + "\nFilterWeight: " + FilterWeight + "\nEnableWatchdog: " + EnableWatchdog +
                              "\nName: " + PointName);
        }

        public class Parameters
        {
            public int PointNumber, ModuleType, PointType;
            public float HighScale, LowScale, Offset, Gain, FilterWeight, WatchdogValue;
            public bool EnableWatchdog;
            public string PointName;
        }
    }
}
