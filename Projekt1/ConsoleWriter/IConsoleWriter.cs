using Projekt1.tape;

namespace Projekt1;

public interface IConsoleWriter
{
    void ShowTapeContent(Tape tape);
    void ShowReadsWritesToDisk(Tape tape3, Tape tape2, Tape tape1);
}