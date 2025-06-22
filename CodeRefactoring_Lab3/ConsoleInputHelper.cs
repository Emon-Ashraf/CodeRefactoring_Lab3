using System;

namespace PersonalFinanceManagement
{
    public static class ConsoleInputHelper
    {
        public static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Invalid input. Try again: ");
            }
            return result;
        }

        public static double ReadDouble(string prompt)
        {
            Console.Write(prompt);
            double result;
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.Write("Invalid input. Try again: ");
            }
            return result;
        }

        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
