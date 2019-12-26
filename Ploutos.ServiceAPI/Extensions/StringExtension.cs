namespace Ploutos.ServiceAPI.Extensions
{
    public static class StringExtension
    {
        public static string NullSafeTrim(this string value)
        {
            if (value != null)
            {
                value = value.Trim();
            }

            return value;
        }
    }
}