using System.Text.RegularExpressions;

namespace Robot.Application.Extensions
{
    public static class NumericTextbox
    {
        public static bool ValidateNumber(string value)
        {
            var regex = @"^([0-9]+)?\.?([0-9]+)?$";
            return Regex.Match(value, regex).Success;
        }
    }
}
