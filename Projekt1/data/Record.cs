using System;

namespace Projekt1.data
{
    public class Record
    {
        private string _person;
        
        public Record(string person)
        {
            _person = person;
        }

        private String GetValue()
        {
            return _person;
        }
        
        public int LexicographicOrder(Record t)
        {
            var person1 = GetValue();
            var person2 = t.GetValue();

            return string.Compare(person1, person2);
        }
    }
}