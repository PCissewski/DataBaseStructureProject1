using System;
using System.IO;
using System.Linq;

namespace Projekt1;

    public class FileReader
    {
        public void ReadRecordsFromFile(string path)
        {
            var files = Directory.GetFiles(path);
            var filesList = files.ToList(); // list of files
            var records = File.ReadAllLines(filesList[0]); // records in one file
            PrintRecords(records);
        }
        
        private string FetchRecord(string[] records)
        {
            var record = "elo";
            return record;
        }

        private static void PrintRecords(string[] records)
        {
            foreach (var rec in records)
            {
                Console.WriteLine(rec);
            }
        }
    }
