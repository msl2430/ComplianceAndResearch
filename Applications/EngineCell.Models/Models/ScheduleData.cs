using System;
using EngineCell.Core.Constants;

namespace EngineCell.Models.Models
{
    public class ScheduleData
    {
        public decimal TimeIntoStage { get; set; }
        public WidgetConstants.TestScheduleSetpointType SetpointType { get; set; }
        public decimal Value { get; set; }

        public ScheduleData() { }

        public ScheduleData(decimal timeIntoStage, decimal value, WidgetConstants.TestScheduleSetpointType setpointType)
        {
            TimeIntoStage = timeIntoStage;
            Value = value;
            SetpointType = setpointType;
        }

        public ScheduleData(string timeIntoStage, string value, WidgetConstants.TestScheduleSetpointType setpointType)
        {
            TimeIntoStage = Convert.ToDecimal(timeIntoStage);
            Value = Convert.ToDecimal(value);
            SetpointType = setpointType;
        }
    }
}
