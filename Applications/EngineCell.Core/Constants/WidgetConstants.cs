using System.Collections.Generic;

namespace EngineCell.Core.Constants
{
    public class WidgetConstants
    {
        public enum Widget
        {
            TestSchedule = 1,
            VentilationControl1,
            VentilationControl2,
            DynoPid,
            Starter,
        }

        public enum WidgetSetting
        {
            TestScheduleFile = 1,
            VentCtrl1InsideThermoCouple,
            VentCtrl1OutsideThermoCouple,
            VentCtrl1AnalogOutput,
            VentCtrl1Gain,
            VentCtrl1SetPoint,
            VentCtrl2InsideThermoCouple,
            VentCtrl2OutsideThermoCouple,
            VentCtrl2AnalogOutput,
            VentCtrl2Gain,
            VentCtrl2SetPoint,
            DynoPidMode,
            DynoPidMeasurement,
            DynoPidSetpoint,
            StarterInitialCrankTime,
            StarterStartParameter,
            StarterAddedCrankTime,
        }

        public enum DynoPidMode
        {
            Motor,
            Absorb,
            MotorAbsorb,
        }

        public enum DynoPidMeasurement
        {
            Rpm,
            Load,
        }

        public enum PhaseEndSetting
        {
            RunTime,
            IterationCount,
            ErrorCount,
        }

        public static IList<string> Widgets = new[]
        {
            "Starter",
            "Dyno PID",
            "Ventilation Control 1",
            "Test Schedule"
        };

        public enum TestScheduleSetpointType
        {
            Dyno,
            Throttle,
        }
    }
}
