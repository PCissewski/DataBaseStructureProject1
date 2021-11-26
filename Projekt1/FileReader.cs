using System;
using System.IO;
using System.Text;

namespace Projekt1;

    public class FileReader
    {
        /// <summary>
        /// Reads single record from a file.
        /// Reads 10 bytes of and stops when CR is found 
        /// </summary>
        /// <param name="path">path to a file</param>
        /// <returns>string of a record</returns>
        public void ReadRecord(string path)
        {
            var data = new byte[50];
            var offset = 0;
            var files = Directory.GetFiles(path);
            var fs = File.Open(files[1], FileMode.Open);
            
            while (true)
            {
                var eof = fs.Read(data, offset, 10);
                if (eof == 0)
                    return;

                var temp = Encoding.ASCII.GetString(data);
                
                if (temp.Contains("\r\n"))
                {
                    var record = temp.Split("\r");
                    
                    var trimmedString = record[1].TrimEnd('\0'); // remove \0 bytes
                    var cont = Encoding.ASCII.GetByteCount(trimmedString);
                    var setBack = cont - 1;
                    fs.Position -= setBack; // offset from which byte read next record
                    offset = -10;
                    Console.WriteLine(record[0]);
                    Array.Clear(data, 0, data.Length);
                    //return record[0];
                }
                
                offset += 10;
            }
        }

        public string ReadNext(int offset)
        {
            return "";
        }
    }
