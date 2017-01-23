using System;
using System.Collections.Generic;
using System.IO;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using EngineCell.Models.Models;

namespace EngineCell.Application.Services.DataServices
{
    public interface IWidgetService
    {
        IList<ScheduleData> ReadTestScheduleFromFile(string fileName);
    }

    public sealed class WidgetService : BaseDataService, IWidgetService
    {
        public IList<ScheduleData> ReadTestScheduleFromFile(string fileName)
        {
            var streamReader = new StreamReader(fileName);

            string line;
            var throttleIndex = -1;
            var dynoIndex = -1;
            var setPoints = new List<ScheduleData>();
            while ((line = streamReader.ReadLine()) != null)
            {
                var lineArray = line.Split(new[] { ",", "\r\n", "\n" }, StringSplitOptions.None);
                if (lineArray[0].ToLower() == "note") //line to find out which setpoint is in which index
                {
                    for (var i = 0; i < lineArray.Length; i++)
                    {
                        if (lineArray[i].ToLower() == "throt" && setPoints.IsNullOrEmpty())
                            throttleIndex = i;
                        if (lineArray[i].ToLower() == "dyno" && setPoints.IsNullOrEmpty())
                            dynoIndex = i;
                    }
                }

                if (throttleIndex < 0 || dynoIndex < 0) //if we haven't set the index of our setpoints then we bail out 
                    continue;

                if (lineArray[0].ToLower() != "data") continue;

                if (throttleIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[throttleIndex], WidgetConstants.TestScheduleSetpointType.Throttle));
                if (dynoIndex >= 0)
                    setPoints.Add(new ScheduleData(lineArray[1], lineArray[dynoIndex], WidgetConstants.TestScheduleSetpointType.Dyno));
            }

            return setPoints;
        }

        public static string GetWidgetName(WidgetConstants.Widget widget)
        {
            switch (widget)
            {
                case WidgetConstants.Widget.TestSchedule:
                    return "Test Schedule";
                    
                case WidgetConstants.Widget.DynoRamp:
                    return "Dyno Ramp";
                case WidgetConstants.Widget.ThrottleRamp:
                    return "Throttle Ramp";
                case WidgetConstants.Widget.CustomChart1:
                    return "Custom Chart 1";
                case WidgetConstants.Widget.CustomChart2:
                    return "Custom Chart 2";
                case WidgetConstants.Widget.CustomChart3:
                    return "Custom Chart 3";
                case WidgetConstants.Widget.CustomChart4:
                    return "Custom Chart 4";
                case WidgetConstants.Widget.CustomChart5:
                    return "Custom Chart 5";
                case WidgetConstants.Widget.VentilationControl1:
                    return "Ventilation Control 1";
                case WidgetConstants.Widget.VentilationControl2:
                    return "Ventilation Control 2";
                case WidgetConstants.Widget.Starter:
                    return "Starter";
                default:
                    throw new ArgumentOutOfRangeException(nameof(widget), widget, null);
            }
        }
    }
}
