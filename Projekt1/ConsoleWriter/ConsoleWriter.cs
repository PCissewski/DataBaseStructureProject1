using Projekt1.tape;

namespace Projekt1;

public class ConsoleWriter : IConsoleWriter
{
    public void ShowTapesContent(int phaseCount, Tape big, Tape small, Tape result)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Phase number {phaseCount}\n");
        Console.ResetColor();
        Console.WriteLine($"Tape Big: ({big.TapeName})");
        Console.WriteLine($"Current number of series: {big.GetSeriesCount()} (includes {big.GetEmptySeriesCount()} empty series)");
        big.PrintRecords();
        Console.WriteLine($"\nTape Small: ({small.TapeName})");
        Console.WriteLine($"Current number of series: {small.GetSeriesCount()} (includes {small.GetEmptySeriesCount()} empty series)");
        small.PrintRecords();
        Console.WriteLine($"\nTape Result: ({result.TapeName})");
        Console.WriteLine($"Current number of series: {result.GetSeriesCount()} (includes {result.GetEmptySeriesCount()} empty series)");
        result.PrintRecords();
        Console.WriteLine("----------------------------------");
    }

    public void ShowTapeContent(Tape tape)
    {
        Console.WriteLine($"\n{tape.TapeName} content:");
        Console.WriteLine($"Current number of series: {tape.GetSeriesCount()} (includes {tape.GetEmptySeriesCount()} empty series)");
        tape.PrintRecords();
        Console.WriteLine("----------------------------------");
    }

    public void ShowReadsWritesToDisk(Tape tape3, Tape tape2, Tape tape1)
    {
        Console.WriteLine($"Number of writes to a disk: {tape1.GetWriteCounter() + tape2.GetWriteCounter() + tape3.GetWriteCounter()}");
        Console.WriteLine($"Number of reads from a disk: {tape1.GetReadCounter() + tape2.GetReadCounter() + tape3.GetReadCounter()}");
    }
}