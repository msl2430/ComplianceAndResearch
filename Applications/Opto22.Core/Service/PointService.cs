using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Opto22.Core.Models;
using Opto22.OptoMMP3;

namespace Opto22.Core.Service
{
    public interface IPointService
    {
        AnalogPointModel GetAnalogPointModel(int pointNumber);
        AnalogPointModel GetAnalogPointModel64(int pointNumber);
        DigitalPointModel GetDigitalPointModel(int pointNumber);
        IList<DigitalPointModel> GetAllDigitalPoints();
        Task<IList<AnalogPointModel>> GetAllPointsAsync();
    }

    public sealed class PointService : IPointService
    {
        private OptoMMP OptoMmp { get; set; }

        public PointService(OptoMMP optoMmp)
        {
            OptoMmp = optoMmp;
        }

        public AnalogPointModel GetAnalogPointModel(int pointNumber)
        {
            var point = new AnalogPointModel.Parameters() {PointNumber = pointNumber};
            Single lower, upper;
            var result = OptoMmp.ReadAnalogPointConfiguration64(pointNumber, out point.ModuleType, out point.PointType, out point.HighScale, out point.LowScale, out point.Offset,
                out point.Gain, out point.FilterWeight, out point.EnableWatchdog, out point.WatchdogValue, out point.PointName);

            if (result != 0)
                throw new Exception("Error reading analog point configuration for point number " + pointNumber);

            return new AnalogPointModel(point);
        }

        public AnalogPointModel GetAnalogPointModel64(int pointNumber)
        {
            var point = new AnalogPointModel.Parameters() { PointNumber = pointNumber };
            Single lower, upper;
            var result = OptoMmp.ReadAnalogPointConfiguration64(pointNumber, out point.ModuleType, out point.PointType, out point.HighScale, out point.LowScale, out point.Offset,
                out point.Gain, out point.FilterWeight, out point.EnableWatchdog, out point.WatchdogValue, out point.PointName);

            if (result != 0)
                throw new Exception("Error reading analog point configuration for point number " + pointNumber);

            return new AnalogPointModel(point);
        }

        public DigitalPointModel GetDigitalPointModel(int pointNumber)
        {
            var point = new DigitalPointModel.Parameters() { PointNumber = pointNumber };
            var result = OptoMmp.ReadDigitalPointConfiguration64(pointNumber, out point.ModuleType, out point.PointType, out point.DigitalFeature, out point.EnableWatchdog, out point.WatchdogValue,
                out point.PointName);
            return new DigitalPointModel(point);
        }

        public IList<DigitalPointModel> GetAllDigitalPoints()
        {
            var points = new List<DigitalPointModel>();
            for (var i = 0; i < 64; i++)
            {
                var point = GetDigitalPointModel(i);
                points.Add(point);
            }
            return points;
        }

        public async Task<IList<AnalogPointModel>> GetAllPointsAsync()
        {
            var points = new List<AnalogPointModel>();
            var pointTasks = new List<Task>();
            for (var i = 0; i < 64; i++)
            {
                var counter = i;
                pointTasks.Add(Task.Factory.StartNew(() => GetAnalogPointModel(counter)).ContinueWith(p => points.Add(p.Result)));
            }

            await Task.WhenAll(pointTasks);

            return points;
        }
    }
}
