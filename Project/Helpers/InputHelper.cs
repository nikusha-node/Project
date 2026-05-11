namespace Project.Helpers;

public static class InputHelper
{
    private static string ReadCentered(string message)
    {
        int screenWidth = Console.WindowWidth;
        int padding = Math.Max(0, (screenWidth - message.Length) / 2);
        Console.Write(new string(' ', padding));
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }

    public static int ReadInt(string message)
    {
        while (true)
        {
            UIHelper.SetColor(ConsoleColor.Cyan);
            var input = ReadCentered($"🎮 {message}");
            UIHelper.ResetColor();

            if (int.TryParse(input, out int result))
                return result;

            UIHelper.WriteLineCentered("⚠️  Invalid number. Try again!", ConsoleColor.Red);
        }
    }

    public static decimal ReadDecimal(string message)
    {
        while (true)
        {
            UIHelper.SetColor(ConsoleColor.Cyan);
            var input = ReadCentered($"💰 {message}");
            UIHelper.ResetColor();

            if (decimal.TryParse(input, out decimal result))
                return result;

            UIHelper.WriteLineCentered("⚠️  Invalid price. Try again!", ConsoleColor.Red);
        }
    }

    public static string ReadString(string message)
    {
        while (true)
        {
            UIHelper.SetColor(ConsoleColor.Cyan);
            var input = ReadCentered($"✏️  {message}");
            UIHelper.ResetColor();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            UIHelper.WriteLineCentered("⚠️  Input cannot be empty. Try again!", ConsoleColor.Red);
        }
    }
}