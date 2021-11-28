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
        private Record _last;
        private FileStream _fs = null;
        private string _fileName;
        
        public Tape(string name)
        {
            _fileName = name;
            FileStream newFile = File.Create(name);
            newFile.Close();
            _fs = File.Open(name, FileMode.Open);
            _pageBuffer = new Page();
            _buffer = _pageBuffer.GetBuffer();
            _last = null;
        }

        public int AddRecord(Record record)
        {
            _last = record;
            if (_pageBuffer.IsFull())
            {
                FlushBufferPage();
            }
            return _pageBuffer.InsertRecord(record);
        }

        public Record GetRecord()
        {
            return _pageBuffer.ReadRecord();
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
        /// </summary>
        public void Flush()
        {
            FlushBufferPage();
            _fs.Flush();
            CloseFile();
        }
        
        /// <summary>
        /// Save buffer content into the file
        /// </summary>
        /// <returns>return 0 on success</returns>
        private void FlushBufferPage()
        {
            _fs.Write(_buffer, 0, _pageBuffer.GetCurrentSize());
            
            _pageBuffer.ClearBuffer();
        }
        
        /// <summary>
        /// Fill buffer with data to read
        /// </summary>
        private void FillBuffer()
        {
            _pageBuffer.ClearBuffer();
            var size = _fs.Read(_buffer, 0, Page.GetMaxRecords() * Record.GetSavedRecordSize());
            _pageBuffer.SetCurrentSize(size);
        }
    }
}