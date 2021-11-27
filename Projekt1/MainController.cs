using System.IO;
using System.Text;
using Projekt1.data;
using Projekt1.tape;

namespace Projekt1
{
    public class MainController
    {
        Tape t1 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/1.txt");
        Tape t2 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/2.txt");
        Tape t3 = new ("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/3.txt");
        
        /// <summary>
        /// load data from the file to a tape 3
        /// </summary>
        public void LoadData()
        {
            FileStream fs = File.Open("X:/InformatykaSemestr5/SBD/Project1/Projekt1/Projekt1/InputFiles/1.txt", FileMode.Open);
            var offset = 0;
            while (true)
            {
                var buffer = new byte[50];
                var eof = fs.Read(buffer, offset, 20);
                if (eof == 0)
                {
                    break;
                }
                var temp = Encoding.ASCII.GetString(buffer);
                if (temp.Contains("\r\n"))
                {
                    var record = temp.Split("\r");
                    var trimmedString = record[1].TrimEnd('\0');
                    
                    var cont = Encoding.ASCII.GetByteCount(trimmedString);
                    var setBack = cont - 1;
                    fs.Position -= setBack;
                    offset = -10;
                    
                    t3.AddRecord(new Record(record[0]));
                }

                offset += 10;

            }
        }
        
    }
}