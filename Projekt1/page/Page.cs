using Projekt1.record;

namespace Projekt1.page
{
    
    public class Page
    {
        private const int PageSize = 100;
        private int _currentSize;
        private int _position;
        private readonly byte[] _pageBuffer;

        public Page()
        {
            _currentSize = 0;
            _position = 0;
            _pageBuffer = new byte[PageSize];
        }

        public static int GetMaxRecordsNumber()
        {
            return PageSize / Record.GetRecordSavedSize();
        }

        public void SetCurrentSize(int size)
        {
            _currentSize = size;
        }

        public void InsertRecord(Record rec)
        {
            if (Record.GetRecordSize() + _currentSize > PageSize)
            {
                return;
            }
            // copy record and the end of currently used buffer
            Array.Copy(rec.GetSaveValue(), 0, _pageBuffer, _currentSize, Record.GetRecordSavedSize());
            _currentSize += Record.GetRecordSavedSize();
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
                var semiColon = _pageBuffer[_position + i];
                counter += 1;
                if (semiColon == 59 || _position + i + 1 == PageSize)
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
            return _currentSize > PageSize - Record.GetRecordSavedSize();
        }

        public bool IsEmpty()
        {
            return _currentSize == _position;
        }

    }
}