using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Projekt1.generator
{
    public class Generator
    {
        /// <summary>
        /// Generate array of bytes with person's name and last name.
        /// </summary>
        /// <param name="args">environmental variable array with absolute path
        /// to the text files containing names and last names
        /// </param>
        /// <returns>single record containing person's name and last name in byte array</returns>
        public string SingleRecord(string[] args)
        {
            var names = File.ReadAllLines(args[1]);
            var lastNames = File.ReadAllLines(args[2]);

            return RandomPerson(names, lastNames);
        }
        /// <summary>
        /// Generate randomly person's name and last name.
        /// </summary>
        /// <param name="names">array of names</param>
        /// <param name="lastNames">array of last names</param>
        /// <returns>string containing person's name and last name</returns>
        private string RandomPerson(string[] names, string[] lastNames)
        {
            var r = new Random();
            
            var rName = r.Next(0, names.Length);
            var rLastName = r.Next(0, lastNames.Length);
            
            // TODO consider adding delimiters like this name:lastName; may come handy in lexicographic sort
            var person = $"{names[rName]} {lastNames[rLastName]}\r\n";
            
            return person;
        }

        public string GenerateTestFile(int recordsNumber, string[] args)
        {
            var path = "X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/testFile.txt";
            File.Delete(path);
            
            StreamWriter sw = File.AppendText(path);
            while (recordsNumber != 0)
            {
                sw.Write(SingleRecord(args));
                recordsNumber--;
            }
            sw.Close();
            return path;
        }
    }
}