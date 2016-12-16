namespace EngineCell.Core.Constants
{
    public class WidgetConstants
    {
        public enum Widget
        {
            VentilationControl1 = 1,
            VentilationControl2,
            DynoPid,
        }

        public enum WidgetSetting
        {
            VentCtrl1Active = 1,
            VentCtrl1InsideThermoCouple,
            VentCtrl1OutsideThermoCouple,
            VentCtrl1AnalogOutput,
            VentCtrl1Gain,
            VentCtrl1SetPoint,
            VentCtrl2Active,
            VentCtrl2InsideThermoCouple,
            VentCtrl2OutsideThermoCouple,
            VentCtrl2AnalogOutput,
            VentCtrl2Gain,
            VentCtrl2SetPoint,
            DynoPidMode,
            DynoPidMeasurement,
            DynoPidSetpoint,
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
    }
}
