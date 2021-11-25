using System;
using Projekt1.data;
using Projekt1.generator;

namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var f = new FileReader();
            var test = f.ReadRecord(args[0]);
            Console.WriteLine(test);
            
        }
    }
