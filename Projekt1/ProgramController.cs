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
            var F_n = 1;
            var F_n1 = 0;
            var F_n2 = 0;
            var seriesCount = 0;
            
            _tape1.DefaultFileSettings();
            _tape2.DefaultFileSettings();

            var tape = _tape1;
            Record rec = null;
            Record prevRecord;
            var wchichTape = true;
            var series = false;
            var continueSeries = false;
            var toMergeRecord = "";
            var merge = false;
            _tape3.OpenFile(); // open file to transfer data to tape 1 and 2
            
            // Read occurs when page buffer is empty
            // Write occurs when we want to flush content into the file
            do
            {
                // od tego miejsca
                rec = _tape3.GetRecord();

                if (rec is null)
                {
                    break;
                }
                
                if (merge)
                {
                    rec.SetValue(toMergeRecord);
                    merge = false;
                }
                
                if (rec != null && !rec.GetValue().Contains(';'))
                {
                    toMergeRecord = rec.GetValue();
                    merge = true;
                    continue;
                }
                // do tego miejsca jest wczytywanie rekordu tak nie wiem
                
                
                tape.AddRecord(rec);
            } while (true);
            _tape1.Flush();
            
            while (_tape3.CanRead() && false)
            {
                prevRecord = tape.GetLastRecord();
                if (!series)
                {
                    rec = _tape3.GetRecord();    
                }

                if (rec != null && !series)
                {
                    seriesCount += 1;
                    series = true;
                    if (seriesCount == F_n)
                    {
                        // next Fibonacci number 
                        F_n2 = F_n1;
                        F_n1 = F_n;
                        F_n = GetNextFibonacci(F_n1, F_n2);
                        tape.SetSeriesCount(seriesCount);
                    
                        // change tape
                        if (wchichTape)
                        {
                            tape = _tape2;
                            wchichTape = false;
                        }
                        else if(!wchichTape)
                        {
                            tape = _tape1;
                            wchichTape = true;
                        }
                    
                        seriesCount = tape.GetSeriesCount(); // get series count of changed tape
                    }
                    continue;
                }
                
                

                if (rec != null)
                {
                    tape.AddRecord(rec);
                    series = false;
                    rec = null;
                }
                
            }
            
            

            
            //_tape1.Flush();
            //_tape2.Flush();
            //_tape3.DefaultFileSettings();
            
        }

        private int GetNextFibonacci(int f1, int f2)
        {
            return f1 + f2;
        }

        public Tape PolyphaseMergeSort()
        {
            return null;
        }
    }
}