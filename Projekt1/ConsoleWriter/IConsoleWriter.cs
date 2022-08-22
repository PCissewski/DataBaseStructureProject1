using Projekt1.tape;

namespace Projekt1;

public interface IConsoleWriter
{
    void ShowTapesContent(int phaseCount, Tape big, Tape small, Tape result);
    void ShowTapeContent(Tape tape);
    void ShowReadsWritesToDisk(Tape tape3, Tape tape2, Tape tape1);
}