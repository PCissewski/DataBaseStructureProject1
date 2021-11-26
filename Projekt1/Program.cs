using System;
using Projekt1.data;
using Projekt1.generator;

namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var f = new FileReader();
            f.ReadRecord(args[0]);

        }
    }
