using System;

namespace CourierService.Input
{
    public static class ConsoleInputReader
    {
        public static double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            if (!double.TryParse(Console.ReadLine(), out double value))
                throw new ArgumentException("Expected a numeric value.");

            return value;
        }

        public static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int value))
                throw new ArgumentException("Expected an integer value.");

            return value;
        }

        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim();
        }
    }
}
