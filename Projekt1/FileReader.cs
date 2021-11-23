using System;
using System.IO;
using System.Linq;

namespace Projekt1;

    public class FileReader
    {
        public void Read(string path)
        {
            var files = Directory.GetFiles(path);
            var output = files.Select(FetchRecord).ToList();
        }
        
        private string FetchRecord(string file)
        {
            Console.WriteLine(file);
            return file;
        }
    }
