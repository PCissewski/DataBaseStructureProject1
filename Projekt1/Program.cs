using Projekt1.generator;

namespace Projekt1
{

    class Program
    {
        static void Main(string[] args)
        {
            var app = new ProgramController(new ConsoleWriter());
            var gen = new Generator(args);
            app.Run(gen.GenerateTestFile(15));
        }

    }
}