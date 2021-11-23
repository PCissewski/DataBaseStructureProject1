namespace Projekt1;

    class Program
    {
        static void Main(string[] args)
        {
            var reader = new FileReader();
            reader.Read(args[0]);
        }
    }
