namespace Projekt1.generator
{
    public class Generator
    {
        private readonly string _testFile;
        private readonly string _names;
        private readonly string _surnames;

        public Generator(string[] inputFiles)
        {
            _names = inputFiles[1];
            _surnames = inputFiles[2];
            _testFile = inputFiles[4];
        }

        private string SingleRecord()
        {
            var names = File.ReadAllLines(_names);
            var surnames = File.ReadAllLines(_surnames);

            return RandomPerson(names, surnames);
        }

        private static string RandomPerson(string[] names, string[] surnames)
        {
            var r = new Random();
            
            var rName = r.Next(0, names.Length);
            var rLastName = r.Next(0, surnames.Length);
            
            // TODO consider adding delimiters like this name:lastName; may come handy in lexicographic sort
            var person = $"{names[rName]} {surnames[rLastName]}\r\n";
            
            return person;
        }

        public string GenerateTestFile(int recordsNumber)
        {
            File.Delete(_testFile);
            
            var sw = File.AppendText(_testFile);
            while (recordsNumber != 0)
            {
                sw.Write(SingleRecord());
                recordsNumber--;
            }
            sw.Close();
            return _testFile;
        }
    }
}