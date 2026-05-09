namespace Project.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        return char.ToUpper(text[0]) + text.Substring(1).ToLower();
    }
}
