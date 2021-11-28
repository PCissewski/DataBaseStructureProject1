using System;
using System.IO;
using System.Text;
using Projekt1;
using Projekt1.data;
using Projekt1.generator;
using Projekt1.tape;

namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var app = new MainController();
            app.LoadData(args[3]);
        }
        
    }
