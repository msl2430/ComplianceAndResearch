
namespace EngineCell.Core.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToDollarString(this decimal amount)
        {
            return amount.ToString("$#,##0.00");
        }
    }
}
