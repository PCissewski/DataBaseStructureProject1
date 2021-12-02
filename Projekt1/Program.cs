using System;
using Projekt1;

namespace Projekt1
{

    class Program
    {
        static void Main(string[] args)
        {
            var app = new ProgramController();

            app.LoadData(args[3]);
            //app.Sort();
            app.SplitBetweenTapes();
            app.PolyphaseMergeSort();
        }

    }
}