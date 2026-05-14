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


    public static string ReadPassword(string message)
    {
        int screenWidth = Console.WindowWidth;
        int padding = Math.Max(0, (screenWidth - message.Length) / 2);
        Console.Write(new string(' ', padding));
        UIHelper.SetColor(ConsoleColor.Cyan);
        Console.Write($"🔒 {message}");
        UIHelper.ResetColor();

        var password = new System.Text.StringBuilder();

        while (true)
        {
            var key = Console.ReadKey(intercept: true); 

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b"); 
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
                Console.Write("*"); 
            }
        }

        if (password.Length == 0)
        {
            UIHelper.WriteLineCentered("⚠️  Password cannot be empty. Try again!", ConsoleColor.Red);
            return ReadPassword(message);
        }

        return password.ToString();
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