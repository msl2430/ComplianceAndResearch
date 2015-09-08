using Robot.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Models.Helpers
{
    public static class ThrottleSetPointHelper
    {
        public static IList<ThrottleSetPointModel> GetAllThrottleSetPoints(ThrottleSetPointChartModel chart)
        {
            var pointList = new List<ThrottleSetPointModel>();
            var chartPoints = chart.SetPoints.OrderBy(sp => sp.Gear).ThenBy(sp => sp.Acceleration).ThenBy(sp => sp.Speed).ToList();

            for (var i = 0; i < chartPoints.Count - 1; i++)
            {
                var startPoint = chartPoints[i];
                var nextPoint = chartPoints[i + 1];
                pointList.Add(new ThrottleSetPointModel
                {
                    ThrottleSetPointId = pointList.Count,
                    ThrottleSetPointChartId = startPoint.ThrottleSetPointChartId,
                    Speed = startPoint.Speed,
                    Gear = startPoint.Gear,
                    Acceleration = startPoint.Acceleration,
                    SetPoint = startPoint.SetPoint
                });

                if (nextPoint.Speed - startPoint.Speed <= 1) continue;

                var slope = Math.Abs((nextPoint.SetPoint - startPoint.SetPoint)/ (nextPoint.Speed - startPoint.Speed));

                for (var j = startPoint.Speed + 1; j < nextPoint.Speed; j++)
                {
                      pointList.Add(new ThrottleSetPointModel()
                      {
                          ThrottleSetPointId = pointList.Count,
                          ThrottleSetPointChartId = startPoint.ThrottleSetPointChartId,
                          Speed = j,
                          Gear = startPoint.Gear,
                          Acceleration = startPoint.Acceleration,
                          SetPoint = (slope * (j - startPoint.Speed)) + startPoint.SetPoint
                      });
                }
            }

            return pointList;
        }

        public static void CalculateSetPointByRoadTest(IList<ThrottleSetPointModel> throttleSetPoints, RoadTestChartModel roadTest)
        {
            
        }
    }
}
