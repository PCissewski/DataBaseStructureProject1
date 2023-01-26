using System.Text;

namespace Projekt1.record
{
    public class Record
    {
        private string _time;
        private static int _recordSize = 5;
        private static int _recordSaveSize = 6;
        /// <summary>
        /// Create record with given Value
        /// </summary>
        /// <param name="time">string of a name of the person</param>
        public Record(string time)
        {
            _time = time;
        }

        public string GetRecord()
        {
            return _time;
        }

        public static int GetRecordSavedSize()
        {
            return _recordSaveSize;
        }

        public static int GetRecordSize()
        {
            return _recordSize;
        }
        
        public static string GetSavedValue(byte[] save)
        {
            return Encoding.ASCII.GetString(save);
        }

        public string GetValue()
        {
            return _time;
        }

        public void SetValue(string value)
        {
            _time = value + _time;
        }
        
        public byte[] GetSaveValue()
        {
            var trimmedString = _time.TrimEnd(';');
            return  Encoding.ASCII.GetBytes($"{trimmedString};");
        }
        
        public int LexicographicOrder(Record t)
        {
            var person1 = GetValue();
            var person2 = t.GetValue();

            return string.CompareOrdinal(person1, person2);
        }

        public int CompareTime(Record t)
        {
            var t1 = DateTime.Parse(GetValue().TrimEnd(';'));
            var t2 = DateTime.Parse(t.GetValue().TrimEnd(';'));
            return DateTime.Compare(t1, t2);
        }
    }
}