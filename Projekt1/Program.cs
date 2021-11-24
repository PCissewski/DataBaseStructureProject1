using Projekt1.generator;

namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var gen = new Generator();
            var record = gen.SingleRecord(args);

            //   var reader = new FileReader();
            // reader.ReadRecordsFromFile(args[0]);
        }
    }
