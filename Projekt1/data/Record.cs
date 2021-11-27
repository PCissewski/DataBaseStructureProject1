using System;
using System.Text;

namespace Projekt1.data
{
    public class Record
    {
        private static string _person;
        /// <summary>
        /// Create record with given Value
        /// </summary>
        /// <param name="person">string of a name of the person</param>
        public Record(string person)
        {
            _person = person;
        }

        public static String GetSavedValue(byte[] save)
        {
            if (save.Length == GetSavedRecordSize())
            {
                return Encoding.ASCII.GetString(save);    
            }

            return null;
        }

        public static int GetRecordSize()
        {
            return Encoding.ASCII.GetByteCount(_person);
        }

        public static int GetSavedRecordSize()
        {
            return 2 + Encoding.ASCII.GetByteCount(_person); // + 2 because of CR and LF bytes
        }

        private String GetValue()
        {
            return _person;
        }

        public byte[] GetSaveValue()
        {
            return  Encoding.ASCII.GetBytes($"{_person}\r\n");
        }
        
        public int LexicographicOrder(Record t)
        {
            var person1 = GetValue();
            var person2 = t.GetValue();

            return string.Compare(person1, person2);
        }
    }
}