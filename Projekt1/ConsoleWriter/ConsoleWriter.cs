using Projekt1.tape;

namespace Projekt1;

public class ConsoleWriter : IConsoleWriter
{
    public void ShowTapeContent(Tape tape)
    {
        Console.WriteLine($"\n{tape.TapeName} content:");
        Console.WriteLine($"Current number of series: {tape.GetSeriesCount()}");
       // tape.PrintRecords();
        Console.WriteLine("----------------------------------");
    }

    public void ShowReadsWritesToDisk(Tape tape3, Tape tape2, Tape tape1)
    {
        Console.WriteLine($"Number of writes to a disk: {tape1.GetWriteCounter() + tape2.GetWriteCounter() + tape3.GetWriteCounter()}");
        Console.WriteLine($"Number of reads from a disk: {tape1.GetReadCounter() + tape2.GetReadCounter() + tape3.GetReadCounter()}");
    }
}