using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Projekt1;

    public class FileReader
    {
        public string ReadRecord(string path)
        {
            var data = new byte[50];
            var offset = 0;
            var files = Directory.GetFiles(path);
            var fs = File.Open(files[0], FileMode.Open);
            
            while (true)
            {
                fs.Read(data, offset, 10);

                var temp = Encoding.ASCII.GetString(data);
                
                if (temp.Contains("\r\n"))
                {
                    var record = temp.Split("\r");
                    return record[0];
                }
                
                offset += 10;
            }
        }
    }
