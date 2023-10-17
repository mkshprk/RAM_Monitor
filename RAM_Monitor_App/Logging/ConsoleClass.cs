namespace Logging
{
    public static class ConsoleClass
    {
        public static void SuccessLog(string log)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteInConsole(log);
        }

        public static void FailureLog(string log)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            WriteInConsole(log);
        }

        public static void InfoLog(string log)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteInConsole(log);
        }

        public static void WarningLog(string log)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteInConsole(log);
        }

        private static void WriteInConsole(string log) => Console.WriteLine(log);

    }
}