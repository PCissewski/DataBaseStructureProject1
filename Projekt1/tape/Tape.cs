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
        private Record last;

        public Tape(string path)
        {
            var fs = File.Open(path, FileMode.Open);
            _pageBuffer = new Page();
            _buffer = new byte[fs.Length];
            fs.Close();
            last = null;
        }

        public int AddRecord(Record record)
        {
            return _pageBuffer.InsertRecord(record);
        }

        public Record GetRecord()
        {
            return _pageBuffer.ReadRecord();
        }

        public void CloseFile(FileStream fs)
        {
            try
            {
                fs.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
}