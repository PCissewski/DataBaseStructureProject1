﻿using System;
using System.IO;
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
        /// called only once at the beginning 
        /// </summary>
        public void LoadData(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open);
            var offset = 0;
            _tape3.DefaultFileSettings();
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
            int fib = 1, fib1 = 0, fib2 = 0;
            _tape1.SetSeriesCount(0);
            _tape2.SetSeriesCount(0);
            _tape1.DefaultFileSettings();
            _tape2.DefaultFileSettings();
            var seriesCounter = 0;

            Tape tape = _tape1;
            Record record = null; 
            Record prevRecord = null;
            var tapeChooser = 0;
            bool newSeries = false;
            bool contSeries = false;
            
            bool merge = false;
            var toMergeRecord = "";
            
            while (_tape3.CanRead() || record != null)
            {
                prevRecord = tape.GetLastRecord();

                if (!newSeries)
                {
                    record = _tape3.GetRecord();
                }
                
                if (merge)
                {
                    record.SetValue(toMergeRecord);
                    merge = false;
                }
                
                if (record != null && !record.GetValue().Contains(';'))
                {
                    toMergeRecord = record.GetValue();
                    merge = true;
                    continue;
                }

                if (record != null && prevRecord != null && record.LexicographicOrder(prevRecord) < 0 && !newSeries)
                {
                    seriesCounter++;
                    newSeries = true;
                    if (seriesCounter == fib)
                    {
                        fib2 = fib1;
                        fib1 = fib;
                        fib = GetNextFibonacci(fib1, fib2);
                        tape.SetSeriesCount(seriesCounter);
                        tapeChooser = (tapeChooser + 1) % 2;
                        tape = (tapeChooser == 0) ? _tape1 : _tape2;
                        seriesCounter = tape.GetSeriesCount();
                    }
                }

                if (record != null)
                {
                    tape.AddRecord(record);
                    newSeries = false;
                    record = null;
                }
                
            }

            seriesCounter++;

            if (fib2 != seriesCounter)
            {
                while (true)
                {
                    if (seriesCounter == fib)
                    {
                        tape.SetSeriesCount(seriesCounter);
                        tapeChooser = (tapeChooser + 1) % 2;
                        tape = (tapeChooser == 0) ? _tape1 : _tape2;
                        break;
                    }
                    tape.EmptySeriesCount();
                    seriesCounter++;
                }
            }
            _tape1.Flush();
            //_tape1.CloseFile();
            _tape2.Flush();
            //_tape2.CloseFile();
            
            Console.WriteLine($"Number of writes to a disk: {_tape1.GetWriteCounter() + _tape2.GetWriteCounter() + _tape3.GetWriteCounter()}");
            Console.WriteLine($"Number of reads from a disk: {_tape1.GetReadCounter() + _tape2.GetReadCounter() + _tape3.GetReadCounter()}");
            _tape3.DefaultFileSettings();
            
        }
        
        public Tape PolyphaseMergeSort()
        {
            var tapeBig = _tape2;
            var tapeSmall = _tape1;
            var tapeResult = _tape3;
            
            tapeBig.OpenFile();
            //tapeResult.OpenFile();
            tapeSmall.OpenFile();

            Record recordBig = null;
            Record recordSmall = null;
            Record prevBig = null;
            Record prevSmall = null;
            Record rec;

            bool wroteRecBig = true;
            bool wroteRecSmall = true;
            bool endBig = false;
            bool endSmall;
            
            // here later add which tape is which now its hard coded
            
            
            // if small tape is empty then file is sorted
            while (tapeSmall.CanRead() || recordSmall != null)
            {
                var mergedSeries = 0;
                while (true)
                {
                    if (wroteRecBig)
                    {
                        prevBig = recordBig;
                        recordBig = tapeBig.GetRecord();
                        wroteRecBig = false;
                    }

                    if (wroteRecSmall)
                    {
                        prevSmall = recordSmall;
                        recordSmall = tapeSmall.GetRecord();
                        wroteRecSmall = false;
                    }
                    
                    endBig = endBig || recordBig == null || (prevBig != null && prevBig.LexicographicOrder(recordBig) > 0);
                    endSmall = recordSmall == null || (prevSmall != null && prevSmall.LexicographicOrder(recordSmall) > 0);
                    
                    if (endBig && endSmall)
                    {
                        prevBig = null;
                        prevSmall = null;
                        endBig = false;
                        mergedSeries += 1;

                        if (!tapeSmall.CanRead() && recordSmall == null)
                        {
                            tapeResult.Flush();
                            break;
                        }
                        continue;
                    }

                    if ((recordBig == null || endBig) && recordSmall != null)
                    {
                        rec = recordSmall;
                    }
                    else if (recordSmall == null || endSmall)
                    {
                        rec = recordBig;
                    }
                    else
                    {
                        rec = (recordBig.LexicographicOrder(recordSmall) <= 0) ? recordBig : recordSmall;
                    }
                    tapeResult.AddRecord(rec);
                    if (rec == recordBig)
                    {
                        wroteRecBig = true;
                    }
                    else
                    {
                        wroteRecSmall = true;
                    }
                }

                Tape temp = tapeBig;
                tapeBig = tapeResult;
                tapeResult = tapeSmall;
                tapeSmall = temp;

                recordSmall = recordBig;
                recordBig = null;
                wroteRecBig = true;

            }
            
            
            return tapeBig;
        }

        public void Sort()
        {
            SplitBetweenTapes();
            Tape tape = PolyphaseMergeSort();
        }
        
        private int GetNextFibonacci(int f1, int f2)
        {
            return f1 + f2;
        }

    }
}