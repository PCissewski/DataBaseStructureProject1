namespace Projekt1.generator
{
    public class Generator
    {
        private readonly string _testFile;
        private readonly string _names;
        private readonly string _surnames;
        private readonly ILogger _logger;

        public Generator(string[] inputFiles, ILogger logger)
        {
            _logger = logger;
            _names = inputFiles[1];
            _surnames = inputFiles[2];
            _testFile = inputFiles[4];
        }

        private string SingleRecord()
        {
            // var names = File.ReadAllLines(_names);
            // var surnames = File.ReadAllLines(_surnames);
            //
            // return RandomPerson(names, surnames);
            return RandomTime();
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

        private static string RandomTime()
        {
            var r = new Random();
            var hours = r.Next(0,23);
            var minutes = r.Next(0, 60);

            var hoursStr = hours.ToString();
            var minutesStr = minutes.ToString();

            if (minutes < 10)
            {
                minutesStr = minutes.ToString();
                minutesStr = $"0{minutesStr}";
            }

            if (hours < 10)
            {
                hoursStr = hours.ToString();
                hoursStr = $"0{hoursStr}";
            }

            return $"{hoursStr}:{minutesStr}\r\n";
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
            _logger.SaveInputTapeContent(_testFile);
            return _testFile;
        }
    }
}