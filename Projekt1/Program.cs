using System;
using System.IO;
using System.Text;
using Projekt1.data;
using Projekt1.generator;
using Projekt1.tape;

namespace Projekt1;

    class Program
    {
        Tape t1 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/1.txt");
        Tape t2 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/2.txt");
        static Tape t3 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/3.txt");
        static void Main(string[] args)
        {
            LoadData();
        }

        public static void LoadData()
        {
            FileStream fs = File.Open("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/1.txt", FileMode.Open);
            while (true)
            {
                var buffer = new byte[Record.GetSavedRecordSize()];
                var counter = fs.Read(buffer, 0, 20);
                var value = Encoding.ASCII.GetString(buffer);
                t3.AddRecord(new Record(value));
            }
        }
    }
