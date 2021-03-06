using System;
using System.IO;
using System.Text;
using Projekt1.record;
using Projekt1.page;

namespace Projekt1.tape
{
    public class Tape
    {
        private Page _pageBuffer;
        private byte[] _buffer;
        private Record _lastRecord;
        private Stream _fs = null;
        private string _fileName;
        private int _writeCounter = 0;
        private int _readCounter = 0;
        
        private int _seriesCount;
        private int _emptySeriesCount;
        
        public Tape(string name)
        {
            _fileName = name;
            _fs = File.Open(name, FileMode.Open, FileAccess.ReadWrite);
            _pageBuffer = new Page();
            _buffer = _pageBuffer.GetBuffer();
            _lastRecord = null;
        }

        public void AddRecord(Record record)
        {
            _lastRecord = record;
            if (_pageBuffer.IsFull(record))
            {
                FlushBufferPage();
            }
            _pageBuffer.InsertRecord(record);
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

        public void OpenFile()
        {
            _fs = File.Open(_fileName, FileMode.Open);
        }
        /// <summary>
        /// Makes the output file readable
        /// </summary>
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
        /// <summary>
        /// Close file to see that something appeared in text file
        /// Note: CloseFile is only to see content of file after Write
        ///       Keep open for the duration of the alg.
        /// </summary>
        public void Flush()
        {
            FlushBufferPage();
            _fs.Flush();
            _fs.Position = 0;
            //CloseFile();
        }
        
        /// <summary>
        /// Save buffer content into the file
        /// write counter
        /// </summary>
        /// <returns>return 0 on success</returns>
        private void FlushBufferPage()
        {
            _writeCounter += 1;
            _fs.Write(_buffer, 0, _pageBuffer.GetCurrentSize());
            
            _pageBuffer.ClearBuffer();
        }
        
        /// <summary>
        /// Fill buffer with data to read
        /// read counter
        /// </summary>
        /// TODO Fix this, count gives fake information, now its hard coded
        private void FillBuffer()
        {
            _readCounter += 1;
            _pageBuffer.ClearBuffer();
            var size = _fs.Read(_buffer, 0, Page.GetPageSize());
            _pageBuffer.SetCurrentSize(size);
        }

        public bool CanRead()
        {
            if (!_pageBuffer.IsEmpty() || _fs.Length - _fs.Position > 0)
            {
                return true;
            }

            return false;
        }

        public void SetSeriesCount(int seriesCount)
        {
            _seriesCount = seriesCount;
        }

        public int GetSeriesCount()
        {
            return _seriesCount;
        }

        public void EmptySeriesCount()
        {
            _emptySeriesCount += 1;
        }

        public bool DecEmptySeriesCount()
        {
            if (_emptySeriesCount > 0)
            {
                _emptySeriesCount -= 1;
                return true;
            }

            return false;
        }

        public void SetPosition(int pos)
        {
            _fs.Position = pos;
        }

        public void DecSeriesCount()
        {
            _seriesCount--;
        }
        
        public void DefaultFileSettings()
        {
            _lastRecord = null;
            _pageBuffer.ClearBuffer();
            //_seriesCount = 0;
            _emptySeriesCount = 0;
            _fs.SetLength(0);
            _fs.Close();
            _fs = File.Open(_fileName, FileMode.Open);
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