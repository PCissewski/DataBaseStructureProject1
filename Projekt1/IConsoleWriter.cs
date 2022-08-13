using Projekt1.tape;

namespace Projekt1;

public interface IConsoleWriter
{
    void WriteTapesContent(int phaseCount, Tape big, Tape small, Tape result);
    void WriteResultTapeContent(Tape result);
    void ShowReadsWritesToDisk(Tape tape3, Tape tape2, Tape tape1);
}