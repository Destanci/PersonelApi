namespace PersonelApi.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str) =>
         string.IsNullOrEmpty(str) || str.Length < 2
         ? str.ToLowerInvariant()
         : char.ToLowerInvariant(str[0]) + str[1..];

        public static string ToPascalCase(this string s)
        {
            var words = s.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(word => word[..1].ToUpper() +
                                         word[1..].ToLower());

            var result = String.Concat(words);
            return result;
        }
    }
}
