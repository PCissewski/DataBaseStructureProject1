using System.Text;
using Projekt1.record;
using Projekt1.page;

namespace Projekt1.tape
{
    public class Tape
    {
        private readonly string _tapeName;
        public string TapeName => _tapeName;
        
        private const string OutputFile =
            "X:\\Studia\\InformatykaSemestr5\\SBD\\Project1\\Projekt1\\Projekt1\\OutputFile\\outputFile.txt";
        
        private readonly Page _pageBuffer;
        private readonly byte[] _buffer;
        private Record _lastRecord;
        private Stream _fs;
        
        private int _writeCounter;
        private int _readCounter;
        
        private int _seriesCount;
        private int _emptySeriesCount;

        private List<Record> records = new ();
        
        public Tape(string tapeName)
        {
            _tapeName = tapeName;
            _fs = File.Open(tapeName, FileMode.Open, FileAccess.ReadWrite);
            _pageBuffer = new Page();
            _buffer = _pageBuffer.GetBuffer();
            _seriesCount = 0;
            DefaultFileSettings();
        }

        public void AddRecord(Record record)
        {
            _lastRecord = record;
            if (_pageBuffer.IsFull(record))
            {
                FlushBufferPage();
            }
            _pageBuffer.InsertRecord(record);
            records.Add(record);
        }

        public void PrintRecords()
        {
            foreach (var record in records)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(record.GetRecord());
                Console.ResetColor();
            }
        }
        public Record GetRecord()
        {
            if (_pageBuffer.IsEmpty())
            {
                FillBuffer();
            }
            
            return _pageBuffer.ReadRecord();
        }

        public Record GetLastRecord()
        {
            return _lastRecord;
        }

        private void OpenFile()
        {
            _fs = File.Open(_tapeName, FileMode.Open);
        }
        public void MakeReadable()
        {
            OpenFile();
            var bt = new byte[_fs.Length];
            var count = _fs.Length;
            var value = (int) count;
            var times = _fs.Read(bt, 0, value);
            var str = Encoding.ASCII.GetString(bt);
            var newStr = str.Replace(';', '\n');
            var newBt = Encoding.ASCII.GetBytes(newStr);
            _fs.SetLength(0);
            _fs.Write(newBt,0,value);
            _fs.Close();
            
            _fs = File.Open(OutputFile, FileMode.Open);
            _fs.Write(newBt, 0, value);
            _fs.Close();
        }
        
        public void CloseFile()
        {
            try
            {
                _fs.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void Flush()
        {
            FlushBufferPage();
            _fs.Flush();
            _fs.Position = 0;
        }
        private void FlushBufferPage()
        {
            _writeCounter += 1;
            _fs.Write(_buffer, 0, _pageBuffer.GetCurrentSize());
            
            _pageBuffer.ClearBuffer();
        }
        private void FillBuffer()
        {
            _readCounter += 1;
            _pageBuffer.ClearBuffer();
            var size = _fs.Read(_buffer, 0, Page.GetPageSize());
            _pageBuffer.SetCurrentSize(size);
        }

        public bool CanRead()
        {
            return !_pageBuffer.IsEmpty() || _fs.Length - _fs.Position > 0;
        }
        public void SetSeriesCount(int seriesCount)
        {
            _seriesCount = seriesCount;
        }

        public int GetSeriesCount()
        {
            return _seriesCount;
        }

        public void IncreaseEmptySeriesCount()
        {
            _emptySeriesCount += 1;
        }

        public int GetEmptySeriesCount()
        {
            return _emptySeriesCount;
        }

        public bool DecreaseEmptySeriesCount()
        {
            if (_emptySeriesCount > 0)
            {
                _emptySeriesCount -= 1;
                return true;
            }

            return false;
        }

        public void DecreaseSeriesCount()
        {
            _seriesCount--;
        }
        
        public void DefaultFileSettings()
        {
            _lastRecord = null;
            _pageBuffer.ClearBuffer();
            _emptySeriesCount = 0;
            _fs.SetLength(0);
            _fs.Close();
            _fs = File.Open(_tapeName, FileMode.Open);
            records.Clear();
        }

        public int GetWriteCounter()
        {
            return _writeCounter;
        }

        public int GetReadCounter()
        {
            return _readCounter;
        }
    }
}