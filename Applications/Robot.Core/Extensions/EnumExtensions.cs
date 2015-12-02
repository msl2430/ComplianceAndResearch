using System;

namespace Robot.Core.Extensions
{
    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumValue)
        {
            return (int)((object)enumValue);
        }
    }
}
