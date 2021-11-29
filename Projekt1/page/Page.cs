﻿using System;
using Projekt1.data;

namespace Projekt1.page
{
    
    public class Page
    {
        private static int _pageSize = 100;
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

        public void SetCurrentSize(int size)
        {
            _currentSize = size;
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
            // przeszukiwac buffer az sie nie znajdzie srednika i wtedy odczytac i do tempa wrzucic
            var i = 0;
            var counter = 0;
            while (true)
            {
                var semiColon = _pageBuffer[i];
                counter += 1;
                if (semiColon == 59)
                {
                    break;
                }

                i += 1;
            }

            var temp = new byte[counter];
            Array.Copy(_pageBuffer, _position, temp, 0, counter);
            _position += counter;
            var value = Record.GetSavedValue(temp);
            
            return new Record(value);
        }

        public int GetCurrentSize()
        {
            return _currentSize;
        }

        public void ClearBuffer()
        {
            _currentSize = 0;
            _position = 0;
        }

        public bool IsFull()
        {
            return _currentSize > _pageSize - Record.GetSavedRecordSize();
        }

        public bool IsEmpty()
        {
            return _currentSize == _position;
        }
        // TODO Fix this 
        public static int GetMaxRecords()
        {
            return _pageSize / Record.GetSavedRecordSize();
        }
        
    }
}