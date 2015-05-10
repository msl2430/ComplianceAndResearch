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
            var result = OptoMmp.ReadAnalogPointConfiguration64(pointNumber, out point.ModuleType, out point.PointType, out point.HighScale, out point.LowScale, out point.Offset,
                out point.Gain, out point.FilterWeight, out point.EnableWatchdog, out point.WatchdogValue, out point.PointName);

            if (result != 0)
                throw new Exception("Error reading analog point configuration for point number " + pointNumber);

            return new AnalogPointConfiguration(point);
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
