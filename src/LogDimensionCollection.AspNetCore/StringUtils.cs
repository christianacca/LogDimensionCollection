namespace CcAcca.LogDimensionCollection.AspNetCore
{
    internal static class StringUtils
    {
        public static string PascalCase(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            if (s.Length == 1)
            {
                return char.ToUpper(s[0]).ToString();
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
