using System.Text;
using Projekt1.record;
using Projekt1.tape;

namespace Projekt1;

public static class DataLoader
{
    public static void LoadData(Tape tape, string testFile)
    {
        var fs = File.Open(testFile, FileMode.Open);
        var offset = 0;
        tape.DefaultFileSettings();

        while (true)
        {
            var buffer = new byte[50];
            var eof = fs.Read(buffer, offset, 20);
            if (eof == 0)
            {
                break;
            }
            var temp= Encoding.ASCII.GetString(buffer);
            if (temp.Contains("\r\n") || temp.Contains("\n") || temp.Contains("\r"))
            {
                var record = temp.Split("\r");
                var trimmedString = record[1].TrimEnd('\0');
                    
                var cont = Encoding.ASCII.GetByteCount(trimmedString);
                var setBack = cont - 1;
                fs.Position -= setBack;
                offset = -10;
                    
                tape.AddRecord(new Record(record[0]));
            }

            offset += 10;

        }
            
        tape.Flush();
    }
}