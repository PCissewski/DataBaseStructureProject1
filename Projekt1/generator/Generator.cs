using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Projekt1.generator
{
    public class Generator
    {
        /*
         * Generate array of bytes with person's name and last name.
         * 
         * @param []args - environmental variable array with absolute path to the text files containing names and last names
         * @return single record containing person's name and last name in byte array
         */
        public byte[] SingleRecord(string[] args)
        {
            var names = File.ReadAllLines(args[1]);
            var lastNames = File.ReadAllLines(args[2]);
            
            return Encoding.ASCII.GetBytes(RandomPerson(names, lastNames));
        }
        /*
         * Generate randomly person's name and last name.
         *
         * @param []names - array of names
         * @param []lastNames - array of last names
         * @return string containing person's name and last name 
         */
        private string RandomPerson(string[] names, string[] lastNames)
        {
            Random r = new Random();
            
            var rName = r.Next(0, names.Length);
            var rLastName = r.Next(0, lastNames.Length);
            
            // TODO consider adding delimiters like this name:lastName; may come handy in lexicographic sort
            var person = $"{names[rName]} {lastNames[rLastName]}";
            
            return person;
        }
    }
}