using System;
using System.IO;
using System.Text;
using Projekt1.record;
using Projekt1.tape;

namespace Projekt1
{
    public class ProgramController
    {
        Tape _tape1 = new ("t1.txt");
        Tape _tape2 = new ("t2.txt");
        Tape _tape3 = new ("t3.txt");

        private int _biggerTape;
        private int _phasesCount;
        private readonly IConsoleWriter _consoleWriter;

        public ProgramController(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }
        
        private void LoadData(string testFile)
        {
            FileStream fs = File.Open(testFile, FileMode.Open);
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
                var temp= Encoding.ASCII.GetString(buffer);
                if (temp.Contains("\r\n") || temp.Contains("\n") || temp.Contains("\r"))
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

        private void SplitBetweenTapes()
        {
            int fib = 1, fib1 = 0, fib2 = 0;
            _tape1.SetSeriesCount(0);
            _tape2.SetSeriesCount(0);
            _tape1.DefaultFileSettings();
            _tape2.DefaultFileSettings();
            var seriesCounter = 0;

            Tape tape = _tape1;
            Record record = null;
            Record prevRecord;
            var tapeChooser = 0;
            bool newSeries = false;
            bool continueSeries = false;

            bool merge = false;
            var toMergeRecord = "";
            
            while (_tape3.CanRead() || record != null)
            {
                prevRecord = tape.GetLastRecord();

                if (!newSeries)
                {
                    record = _tape3.GetRecord();
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
                }
                else if (prevRecord != null)
                {
                    continueSeries = record is not null;
                }

                if (record != null && prevRecord != null && record.LexicographicOrder(prevRecord) < 0 && !newSeries)
                {
                    if (continueSeries)
                    {
                        continueSeries = false;
                        newSeries = true;
                    }
                    else
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
                        continue;
                    }

                    
                }

                if (record != null)
                {
                    tape.AddRecord(record);
                    newSeries = false;
                    record = null;
                }
                
            }
            if(!continueSeries)
                seriesCounter++;

            if (!(continueSeries && fib2 == seriesCounter))
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

            _biggerTape = tape == _tape1 ? 1 : 0;
            
            _tape1.Flush();
            _tape2.Flush();

            _tape3.DefaultFileSettings();
        }
        
        private Tape PolyPhaseMergeSort()
        {
            Tape tapeBig;
            Tape tapeSmall;
            var tapeResult = _tape3;

            if (_biggerTape == 1)
            {
                tapeBig = _tape2;
                tapeSmall = _tape1;
            }
            else if (_biggerTape == 0)
            {
                tapeBig = _tape1;
                tapeSmall = _tape2;
            }
            else
            {
                return null;
            }
      
            Record recordBig = null;
            Record recordSmall = null;
            Record prevRecordBig = null;
            Record prevRecordSmall = null;
            Record rec = null;

            bool wroteRecordToBig = true;
            bool wroteRecSmall = true;
            bool endBig = false;
            bool endSmall;
            bool merge = false;
            bool smallMerge = false;
            var toMergeRecord = "";
            

            while (tapeSmall.CanRead() || recordSmall != null || tapeBig.GetSeriesCount() != 1)
            {
                
                var mergedSeries = 0;
                _consoleWriter.WriteTapesContent(_phasesCount, tapeBig, tapeSmall, tapeResult);
                _phasesCount++;
                // merge series
                while (true)
                {
                    if (!endBig && tapeBig.DecEmptySeriesCount()) {
                        endBig = true;
                    }

                    if (wroteRecordToBig)
                    {
                        prevRecordBig = recordBig;
                        recordBig = tapeBig.GetRecord();
                        if (merge)
                        {
                            recordBig.SetValue(toMergeRecord);
                            merge = false;
                        }
                
                        if (recordBig != null && !recordBig.GetValue().Contains(';'))
                        {
                            toMergeRecord = recordBig.GetValue();
                            merge = true;
                            continue;
                        }
                        wroteRecordToBig = false;
                    }

                    if (wroteRecSmall)
                    {
                        prevRecordSmall = recordSmall;
                        recordSmall = tapeSmall.GetRecord();
                        if (merge)
                        {
                            recordSmall.SetValue(toMergeRecord);
                            merge = false;
                        }
                
                        if (recordSmall != null && !recordSmall.GetValue().Contains(';'))
                        {
                            toMergeRecord = recordSmall.GetValue();
                            merge = true;
                            continue;
                        }
                        wroteRecSmall = false;
                    }

                    if (endBig || recordBig == null ||
                        (prevRecordBig != null && prevRecordBig.LexicographicOrder(recordBig) > 0))
                    {
                        endBig = true;
                    }
                    else
                    {
                        endBig = false;
                    }

                    if (recordSmall == null ||
                        (prevRecordSmall != null && prevRecordSmall.LexicographicOrder(recordSmall) > 0))
                    {
                        endSmall = true;
                    }
                    else
                    {
                        endSmall = false;
                    }
                    
                    if (endBig && endSmall)
                    {
                        prevRecordBig = null;
                        prevRecordSmall = null;
                        endBig = false;
                        //merged series
                        mergedSeries++;
                        
                        if (!tapeSmall.CanRead() && recordSmall == null)
                        {
                            tapeResult.Flush();
                            Console.WriteLine($"Merged series: {mergedSeries}");
                            _consoleWriter.WriteResultTapeContent(tapeResult);
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
                        wroteRecordToBig = true;
                    }
                    else
                    {
                        wroteRecSmall = true;
                    }
                }
                tapeSmall.DefaultFileSettings();
                tapeSmall.DecSeriesCount();

                var temp = tapeBig;
                tapeBig = tapeResult;
                tapeResult = tapeSmall;
                tapeSmall = temp;

                recordSmall = recordBig;
                recordBig = null;
                wroteRecordToBig = true;

            }
            
            Console.WriteLine($"Number of phases: {_phasesCount}");
            
            tapeSmall.DefaultFileSettings();
            tapeResult.Flush();
            tapeResult.CloseFile();
            tapeSmall.Flush();
            tapeSmall.CloseFile();
            
            tapeBig.Flush();
            tapeBig.CloseFile();
            _consoleWriter.ShowReadsWritesToDisk(_tape3, _tape2, _tape1);
            return tapeBig;
        }

        private void Sort()
        {
            SplitBetweenTapes();
            //var sortedTape = PolyPhaseMergeSort();
            //sortedTape.MakeReadable();
        }

        public void Run(string testFile)
        {
            LoadData(testFile);
            Sort();
        }

        private int GetNextFibonacci(int f1, int f2)
        {
            return f1 + f2;
        }
    }
}