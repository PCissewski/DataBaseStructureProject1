using System.Text;
using Projekt1.record;
using Projekt1.tape;

namespace Projekt1;

public static class DataLoader
{
    public static void LoadData(Tape tape, string testFile)
    {
        var fs = File.Open(testFile, FileMode.Open);
        tape.DefaultFileSettings();

        while (true)
        {
            var buffer = new byte[Record.GetRecordSavedSize() + 1];
            var eof = fs.Read(buffer, 0, Record.GetRecordSavedSize() + 1);
            if (eof == 0)
            {
                break;
            }
            var temp= Encoding.ASCII.GetString(buffer);
            var trim = temp.TrimEnd('\n');
            temp = trim.TrimEnd('\r');
            tape.AddRecord(new Record(temp));
            
        }
            
        tape.Flush();
    }
}