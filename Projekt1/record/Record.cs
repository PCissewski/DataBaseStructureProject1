using System.Text;

namespace Projekt1.record
{
    public class Record
    {
        private string _person;
        /// <summary>
        /// Create record with given Value
        /// </summary>
        /// <param name="person">string of a name of the person</param>
        public Record(string person)
        {
            _person = person;
        }

        public string GetRecord()
        {
            return _person;
        }
        
        public static String GetSavedValue(byte[] save)
        {
            return Encoding.ASCII.GetString(save);
        }
        
        public int GetRecordSize()
        {
            return Encoding.ASCII.GetByteCount(_person);
        }
        /// <summary>
        /// Get size of a saved record, +1 because of ';' delimiter
        /// </summary>
        /// <returns>number of bytes to save</returns>
        public int GetSavedRecordSize()
        {
            var containsSemiColon = _person.Contains(';');

            if (containsSemiColon)
            {
                return Encoding.ASCII.GetByteCount(_person);
            }
            return 1 + Encoding.ASCII.GetByteCount(_person);
        }

        public String GetValue()
        {
            return _person;
        }

        public void SetValue(string value)
        {
            _person = value + _person;
        }
        
        public byte[] GetSaveValue()
        {
            var trimmedString = _person.TrimEnd(';');
            return  Encoding.ASCII.GetBytes($"{trimmedString};");
        }
        
        public int LexicographicOrder(Record t)
        {
            var person1 = GetValue();
            var person2 = t.GetValue();

            return string.CompareOrdinal(person1, person2);
        }
    }
}