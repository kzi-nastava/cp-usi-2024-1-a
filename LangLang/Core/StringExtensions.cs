namespace LangLang.Core;

public static class StringExtensions
{
    public static string Truncate(this string value, int maxLength)
    {
        return value.Length > maxLength ? value.Substring(0, maxLength) : value;
    }
}