using System;
using System.IO;
using System.Linq;

namespace Projekt1;

    public class FileReader
    {
        public void Read(string path)
        {
            var files = Directory.GetFiles(path);
            Array.ForEach(files,Console.WriteLine);
        }
        
        private static string FetchRecord(string file)
        {
            
            return file;
        }
    }
