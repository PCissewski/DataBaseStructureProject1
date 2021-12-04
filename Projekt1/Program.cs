using System;
using Projekt1;
using Projekt1.generator;

namespace Projekt1
{

    class Program
    {
        static void Main(string[] args)
        {
            var app = new ProgramController();
            var gen = new Generator();
            app.Run(gen.GenerateTestFile(10000,args));
        }

    }
}