using Projekt1.generator;

namespace Projekt1
{
    internal static class Program
    {
        private const string LoggerFile =
            "X:\\Studia\\InformatykaSemestr5\\SBD\\Project1\\Projekt1\\Projekt1\\Logger\\inputTapeContent.txt";
        static void Main(string[] args)
        {
            var app = new ProgramController(new ConsoleWriter());
            var gen = new Generator(args, new Logger());
            //app.Run(gen.GenerateTestFile(15));
            app.Run(LoggerFile);
        }

    }
}