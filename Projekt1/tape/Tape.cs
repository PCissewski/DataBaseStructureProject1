using System;
using System.IO;
using Projekt1.data;
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
        private int _seriesCount;
        private int _emptySeriesCount;
        
        public Tape(string name)
        {
            _fileName = name;
            //FileStream newFile = File.Create(name);
            //newFile.Close();
            _fs = File.Open(name, FileMode.Open, FileAccess.ReadWrite);
            _pageBuffer = new Page();
            _buffer = _pageBuffer.GetBuffer();
            _lastRecord = null;
        }

        public void AddRecord(Record record)
        {
            _lastRecord = record;
            if (_pageBuffer.IsFull())
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

        private void CloseFile()
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
            CloseFile();
        }
        
        /// <summary>
        /// Save buffer content into the file
        /// write counter
        /// </summary>
        /// <returns>return 0 on success</returns>
        private void FlushBufferPage()
        {
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
            _pageBuffer.ClearBuffer();
            var size = _fs.Read(_buffer, 0, 100);
            _pageBuffer.SetCurrentSize(size);
        }

        public bool CanRead()
        {
            if (!_pageBuffer.IsEmpty() || _fs.Length > 0)
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

        public void DefaultFileSettings()
        {
            _lastRecord = null;
            _pageBuffer.ClearBuffer();
            _seriesCount = 0;
            _emptySeriesCount = 0;
        }
        
    }
}