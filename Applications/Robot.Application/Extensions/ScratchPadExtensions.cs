using System.Collections.Generic;
using System.Linq;
using Opto22.Core.Models;
using Robot.Application.Session;
using Robot.Core.Constants;
using Robot.Models.Models;

namespace Robot.Application.Extensions
{
    public static class ScratchPadExtensions
    {
        public static void Update<T>(this ScratchPadModel<T> obj)
        {
            ApplicationSession.ScratchPadFactory.SetScratchPadValue(obj);
        }

        public static void Update<T>(this IScratchPadModel<T> obj)
        {
            ApplicationSession.ScratchPadFactory.SetScratchPadValue(obj);
        }

        public static List<List<IScratchPadModel<decimal>>> ToThrottleSetPointScratchPad(this ThrottleSetPointChartModel tspModel)
        {
            var setPointsByGear = new List<List<IScratchPadModel<decimal>>>();

            foreach (var gearNumber in tspModel.SetPoints.Select(sp => sp.Gear).Distinct().OrderBy(x => x))
            {
                var setPointList = new List<IScratchPadModel<decimal>>();
                var speedPointIndex = 0;
                var accelerationPointIndex = 0;
                for (var i = 0; i < tspModel.SetPoints.Count(sp => sp.Gear == gearNumber); i++)
                {
                    if (i > 1 && i%DataConstants.AccelerationPoints.Length == 0)
                    {
                        speedPointIndex++;
                        accelerationPointIndex = 0;
                    }

                    var point = tspModel.SetPoints.Where(sp => sp.Gear == gearNumber).OrderBy(sp => sp.Speed).ThenBy(sp => sp.Acceleration).ElementAt(i);
                    setPointList.Add(new ScratchPadModel<decimal>(i,
                        "Gear: " + gearNumber + " Speed: " + DataConstants.SpeedPoints[speedPointIndex] + " Acceleration: " + DataConstants.AccelerationPoints[accelerationPointIndex], point.SetPoint));
                    accelerationPointIndex++;
                }
                setPointsByGear.Add(setPointList);
            }

            return setPointsByGear;
        }
    }
}
