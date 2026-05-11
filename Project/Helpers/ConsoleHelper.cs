namespace Project.Helpers;

public static class ConsoleHelper
{
    public static void Header(string title)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("========================================");
        Console.WriteLine($"          {title.ToUpper()}");
        Console.WriteLine("========================================");

        Console.ResetColor();
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine(message);

        Console.ResetColor();
    }

    public static void Pause()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.WriteLine("\nPress any key to continue...");

        Console.ResetColor();

        Console.ReadKey();
    }
}