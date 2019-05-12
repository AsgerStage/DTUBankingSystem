namespace DtuNetbank.Models.Extensions
{
    public static class ExtensionMethods
    {
        #region Decimal Extensions
        public static string ToViewFormat(this decimal value, string format = "N2")
        {
            return value.ToString(format);
        }

        #endregion
    }
}