namespace Backend
{
    using System;

    public static class Term
    {
        private static readonly object guarder = new object();
        public static void Error(object o)
        {
            lock (guarder)
            {
                Head("ERROR", ConsoleColor.Red);
                Console.WriteLine(o);
            }
        }
        public static void Success(object o)
        {
            lock (guarder)
            {
                Head("SUCCESS", ConsoleColor.Green);
                Console.WriteLine(o);
            }
        }
        public static void Warn(object o)
        {
            lock (guarder)
            {
                Head("WARN", ConsoleColor.Yellow);
                Console.WriteLine(o);
            }
        }
        private static void Head(string headType, ConsoleColor color)
        {
            Console.Write("[");
            Console.ForegroundColor = color;
            Console.Write(headType);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]: ");
        }
    }
}