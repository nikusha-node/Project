namespace Project.Helpers;

public static class UIHelper
{
    public static void SetColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    public static void ResetColor()
    {
        Console.ResetColor();
    }

    private static int GetVisualWidth(string text)
    {
        int width = 0;
        foreach (char c in text)
        {
            bool isWide = c >= 0x1100 && (
                c <= 0x115F ||
                c == 0x2329 || c == 0x232A ||
                (c >= 0x2E80 && c <= 0x303E) ||
                (c >= 0x3040 && c <= 0xA4CF) ||
                (c >= 0xAC00 && c <= 0xD7A3) ||
                (c >= 0xF900 && c <= 0xFAFF) ||
                (c >= 0xFE10 && c <= 0xFE1F) ||
                (c >= 0xFE30 && c <= 0xFE6F) ||
                (c >= 0xFF00 && c <= 0xFF60) ||
                (c >= 0xFFE0 && c <= 0xFFE6)
            );
            width += isWide ? 2 : 1;
        }
        return width;
    }

    public static void WriteCentered(string text, ConsoleColor color = ConsoleColor.White)
    {
        SetColor(color);
        foreach (var line in text.Split('\n'))
        {
            var cleanLine = line.TrimEnd('\r');
            int padding = Math.Max(0, (Console.WindowWidth - GetVisualWidth(cleanLine)) / 2);
            Console.WriteLine(new string(' ', padding) + cleanLine);
        }
        ResetColor();
    }

    public static void WriteLineCentered(string text, ConsoleColor color = ConsoleColor.White)
    {
        SetColor(color);
        int padding = Math.Max(0, (Console.WindowWidth - GetVisualWidth(text)) / 2);
        Console.WriteLine(new string(' ', padding) + text);
        ResetColor();
    }

    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
    {
        SetColor(color);
        Console.WriteLine(text);
        ResetColor();
    }

    public static void Divider()
    {
        WriteLineCentered("· · · ════════════════════════ · · ·", ConsoleColor.DarkCyan);
    }

    public static void Success(string message)
    {
        WriteLineCentered($"✅  {message}", ConsoleColor.Green);
    }

    public static void Error(string message)
    {
        WriteLineCentered($"❌  {message}", ConsoleColor.Red);
    }

    public static void Info(string message)
    {
        WriteLineCentered($"💡  {message}", ConsoleColor.Cyan);
    }

    public static void Warning(string message)
    {
        WriteLineCentered($"⚠️   {message}", ConsoleColor.Yellow);
    }

    public static string ReadLineCentered(string prompt, ConsoleColor color = ConsoleColor.White)
    {
        SetColor(color);
        int padding = Math.Max(0, (Console.WindowWidth - prompt.Length) / 2);
        Console.Write(new string(' ', padding) + prompt);
        ResetColor();
        return Console.ReadLine() ?? "";
    }
}