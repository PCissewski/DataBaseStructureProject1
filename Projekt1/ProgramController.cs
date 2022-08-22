using Projekt1.record;
using Projekt1.tape;

namespace Projekt1
{
    public class ProgramController
    {
        private readonly Tape _tape1 = new ("t1.txt");
        private readonly Tape _tape2 = new ("t2.txt");
        private readonly Tape _tape3 = new ("t3.txt");

        private int _biggerTape;
        private int _phasesCount;
        private readonly IConsoleWriter _consoleWriter;

        public ProgramController(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        private void SplitBetweenTapes()
        {
            int fib = 1, fib1 = 0, fib2 = 0;
            var seriesCounter = 0;

            var tape = _tape1;
            
            Record record = null;
            var tapeChooser = 0;
            bool newSeries = false;
            bool continueSeries = false;

            bool merge = false;
            var toMergeRecord = "";
            
            while (_tape3.CanRead() || record != null)
            {
                var prevRecord = tape.GetLastRecord();

                if (!newSeries)
                {
                    record = _tape3.GetRecord();
                    
                    if (merge)
                    {
                        record.SetValue(toMergeRecord);
                        merge = false;
                    }
                
                    if (!record.GetValue().Contains(';'))
                    {
                        toMergeRecord = record.GetValue();
                        merge = true;
                        continue;
                    }
                    
                }
                else if (prevRecord != null && continueSeries)
                {
                    continueSeries = record is not null ;
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

                if (record == null) continue;
                tape.AddRecord(record);
                newSeries = false;
                record = null;

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
                    tape.IncreaseEmptySeriesCount();
                    seriesCounter++;
                }
            }

            _biggerTape = tape == _tape1 ? 1 : 0;
            
            _tape1.Flush();
            _tape2.Flush();
            
            _consoleWriter.ShowTapeContent(_tape1);
            _consoleWriter.ShowTapeContent(_tape2);

            _tape3.DefaultFileSettings();
        }
        
        private Tape PolyPhaseMergeSort()
        {
            Tape tapeBig;
            Tape tapeSmall;
            var tapeResult = _tape3;

            switch (_biggerTape)
            {
                case 1:
                    tapeBig = _tape2;
                    tapeSmall = _tape1;
                    break;
                case 0:
                    tapeBig = _tape1;
                    tapeSmall = _tape2;
                    break;
                default:
                    return new Tape("failTape.txt");
            }
      
            Record recordBig = null;
            Record recordSmall = null;
            Record prevRecordBig = null;
            Record prevRecordSmall = null;
            Record rec;

            bool wroteRecordToBig = true;
            bool wroteRecSmall = true;
            bool endBig = false;
            bool endSmall;
            bool merge = false;
            bool smallMerge = false;
            var toMergeRecord = "";
            

            while (tapeSmall.CanRead())
            {
                
                var mergedSeries = 0;
                _consoleWriter.ShowTapesContent(_phasesCount, tapeBig, tapeSmall, tapeResult);
                _phasesCount++;
                // merge series
                while (true)
                {
                    if (!endBig && tapeBig.DecreaseEmptySeriesCount()) {
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
                            _consoleWriter.ShowTapeContent(tapeResult);
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
                tapeSmall.DecreaseSeriesCount();

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
            // var sortedTape = PolyPhaseMergeSort();
            // sortedTape.MakeReadable();
        }

        public void Run(string outputFile)
        {
            DataLoader.LoadData(_tape3, outputFile);
            Sort();
        }

        private int GetNextFibonacci(int f1, int f2)
        {
            return f1 + f2;
        }
    }
}