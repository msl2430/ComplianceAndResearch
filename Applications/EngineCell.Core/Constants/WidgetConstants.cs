using System.Collections.Generic;

namespace EngineCell.Core.Constants
{
    public class WidgetConstants
    {
        public enum Widget
        {
            TestSchedule = 1,
            CustomChart1,
            CustomChart2,
            CustomChart3,
            CustomChart4,
            CustomChart5,
            VentilationControl1,
            VentilationControl2,
            DynoPid,
            Starter,
        }

        public enum WidgetSetting
        {
            TestScheduleFile = 1,
            TestSscheduleTimeout,
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

        public enum TriggerResultType
        {
            GoToPhase = 1,
            Shutdown,
        }

        public static IList<string> Widgets = new[]
        {
            "Test Schedule",
            "Custom Chart 1",
            "Custom Chart 2",
            "Custom Chart 3",
            "Custom Chart 4",
            "Custom Chart 5",
        };

        public enum TestScheduleSetpointType
        {
            Dyno,
            Throttle,
        }
    }
}
