using System.Collections.Generic;

namespace EngineCell.Core.Constants
{
    public class WidgetConstants
    {
        public enum Widget
        {
            TestSchedule = 1,
            DynoRamp,
            ThrottleRamp,
            CustomChart1,
            CustomChart2,
            CustomChart3,
            CustomChart4,
            CustomChart5,
            VentilationControl1,
            VentilationControl2,
            Starter,
        }

        public enum WidgetSetting
        {
            TestScheduleFile = 1,
            TestScheduleTimeout,
            DynoRampSetpoint,
            DynoRampTime,
            ThrottleRampSetpoint,
            ThrottleRampTime,
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
            StarterCrankTime,
            StarterStartRpm,
            StarterTimeBetweenTries,
            StarterNumberOfTries,
            StarterTimeAtRpm,
            StarterTimeout,
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

        public enum TriggerResultType
        {
            GoToPhase = 1,
            Shutdown,
        }

        public static IList<string> Widgets = new[]
        {
            "Test Schedule",
            "Dyno Ramp",
            "Throttle Ramp",
            "Custom Chart 1",
            "Custom Chart 2",
            "Custom Chart 3",
            "Custom Chart 4",
            "Custom Chart 5",
            "Starter",
        };

        public enum TestScheduleSetpointType
        {
            Dyno,
            Throttle,
        }
    }
}
