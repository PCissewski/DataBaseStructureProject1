// See https://aka.ms/new-console-template for more information

using System;
using Projekt1;

namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var reader = new FileReader();
            reader.Read(args[0]);
        }
    }
