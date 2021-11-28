﻿using System.IO;
using System.Text;
using Projekt1.data;
using Projekt1.tape;

namespace Projekt1
{
    public class ProgramController
    {
        Tape _tape1 = new ("t1.txt");
        Tape _tape2 = new ("t2.txt");
        Tape _tape3 = new ("t3.txt");
        
        /// <summary>
        /// load data from the file to a tape 3
        /// </summary>
        public void LoadData(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open);
            var offset = 0;
            
            while (true)
            {
                var buffer = new byte[50];
                var eof = fs.Read(buffer, offset, 20);
                if (eof == 0)
                {
                    break;
                }
                var temp = Encoding.ASCII.GetString(buffer);
                if (temp.Contains("\r\n"))
                {
                    var record = temp.Split("\r");
                    var trimmedString = record[1].TrimEnd('\0');
                    
                    var cont = Encoding.ASCII.GetByteCount(trimmedString);
                    var setBack = cont - 1;
                    fs.Position -= setBack;
                    offset = -10;
                    
                    _tape3.AddRecord(new Record(record[0]));
                }

                offset += 10;

            }
            
            _tape3.Flush();
        }
        
        public void SplitBetweenTapes()
        {
            var F_n = 1;
            var F_n1 = 0;
            var F_n2 = 0;
            var seriesCount = 0;

            var tape = _tape1;
            Record rec = null;
            Record prevRecord;

            while (_tape3.CanRead())
            {
                prevRecord = tape.GetLastRecord();
            }

        }

        public Tape PolyphaseMergeSort()
        {
            return null;
        }
    }
}