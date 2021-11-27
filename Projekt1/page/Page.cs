using System;
using Projekt1.data;

namespace Projekt1.page
{
    
    public class Page
    {
        private int _pageSize = 100;
        private int _currentSize;
        private int _position;

        private byte[] _pageBuffer;

        public Page()
        {
            _currentSize = 0;
            _position = 0;
            _pageBuffer = new byte[_pageSize];
        }

        public void SetPageSize(int size)
        {
            _pageSize = size;
        }
        /// <summary>
        /// Insert one record
        /// </summary>
        /// <param name="rec">record to insert</param>
        /// <returns>0 on success and -1 when there is no space in buffer</returns>
        public int InsertRecord(Record rec)
        {
            if (Record.GetRecordSize() + _currentSize > _pageSize)
            {
                return -1;
            }
            // copy record and the end of currently used buffer
            Array.Copy(rec.GetSaveValue(), 0, _pageBuffer, _currentSize, Record.GetSavedRecordSize());
            _currentSize += Record.GetSavedRecordSize();
            return 0;
        }

        public byte[] GetBuffer()
        {
            return _pageBuffer;
        }
        public Record ReadRecord()
        {
            if (_currentSize == _position)
            {
                return null;
            }

            var temp = new byte[Record.GetSavedRecordSize()];
            Array.Copy(_pageBuffer, _position, temp, 0, Record.GetSavedRecordSize());
            _position += Record.GetSavedRecordSize();
            var value = Record.GetSavedValue(temp);
            
            return new Record(value);
        }
        
    }
}