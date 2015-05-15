using System;
using System.Collections;
using System.Collections.Generic;
using Opto22.OptoMMP3;

namespace Opto22TestProject
{
    public sealed class PointConfigurationService
    {
        private OptoMMP OptoMmp { get; set; }

        public PointConfigurationService(OptoMMP optoMmp)
        {
            OptoMmp = optoMmp;
        }

        public AnalogPointConfiguration GetAnalogPointConfiguration(int pointNumber)
        {
            var point = new AnalogPointConfiguration.Parameters() {PointNumber = pointNumber};
            Single lower, upper;
            var result = OptoMmp.ReadAnalogPointConfiguration4096(pointNumber, out point.PointType, out point.HighScale, out point.LowScale, out point.Offset,
                out point.Gain, out point.FilterWeight, out point.EnableWatchdog, out point.WatchdogValue, out point.PointName, out lower, out upper);

            if (result != 0)
                throw new Exception("Error reading analog point configuration for point number " + pointNumber);

            return new AnalogPointConfiguration(point);
        }

        public AnalogPointConfiguration GetAnalogPointConfiguration64(int pointNumber)
        {
            var point = new AnalogPointConfiguration.Parameters() { PointNumber = pointNumber };
            Single lower, upper;
            var result = OptoMmp.ReadAnalogPointConfiguration64(pointNumber, out point.ModuleType,out point.PointType, out point.HighScale, out point.LowScale, out point.Offset,
                out point.Gain, out point.FilterWeight, out point.EnableWatchdog, out point.WatchdogValue, out point.PointName);

            if (result != 0)
                throw new Exception("Error reading analog point configuration for point number " + pointNumber);

            return new AnalogPointConfiguration(point);
        }

        public DigitalPointConfiguration GetDigitalPointConfiguration(int pointNumber)
        {
            var point = new DigitalPointConfiguration.Parameters() { PointNumber = pointNumber };
            var result = OptoMmp.ReadDigitalPointConfiguration64(pointNumber, out point.ModuleType, out point.PointType, out point.DigitalFeature, out point.EnableWatchdog, out point.WatchdogValue,
                out point.PointName);
            return new DigitalPointConfiguration(point);
        }

        public IList<DigitalPointConfiguration> GetAllDigitalPoints()
        {
            var points = new List<DigitalPointConfiguration>();
            for (var i = 0; i < 64; i++)
            {
                var point = GetDigitalPointConfiguration(i);
                points.Add(point);
            }
            return points;
        }

        public IList<AnalogPointConfiguration> GetAllPoints()
        {
            var points = new List<AnalogPointConfiguration>();
            for (var i = 0; i < 64; i++)
            {
                var point = GetAnalogPointConfiguration(i);
                points.Add(point);
            }
            return points;
        }
    }
}
