namespace Project.Helpers;

public static class InputHelper
{
    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (int.TryParse(input, out int result))
                return result;

            Console.WriteLine("Invalid number. Try again.");
        }
    }

    public static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal result))
                return result;

            Console.WriteLine("Invalid price. Try again.");
        }
    }

    public static string ReadString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("Input cannot be empty.");
        }
    }
}
