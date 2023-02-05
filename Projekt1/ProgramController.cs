using Projekt1.record;
using Projekt1.tape;

namespace Projekt1
{
    public class ProgramController
    {
        private readonly Tape _tapeA = new ("tA.txt");
        private readonly Tape _tapeB = new ("tB.txt");
        private readonly Tape _tapeC = new ("tC.txt");
        
        private int _phasesCount;
        private readonly IConsoleWriter _consoleWriter;

        public ProgramController(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        private void SplitBetweenTapes(Tape tapeA, Tape tapeB, Tape tapeC)
        {
            var AorB = true; // true - wpisujemy do taśmy A, false wpisujemy do taśmy B

            Record record = null;
            Record previousRecord = null;
            var seriesCount = 0;
            // flaga oznaczajaca że ostatnio sie zmienila tasma na ktora zapisujemy
            bool recentlyChangedTape = false;

            // flaga recently changed ponieważ na końcu odczytu zostaje ostatni rekord
            while (tapeC.CanRead() || recentlyChangedTape)
            {
                // pobranie poprzedniego rekordu z tasmy do ktorej obecnie zapisujemy
                // aby porownac z obecnym rekordem z tasmy C czy nadal jest seria
                previousRecord = AorB ? tapeA.GetLastRecord() : tapeB.GetLastRecord();

                if (!recentlyChangedTape) 
                {
                    record = tapeC.GetRecord(); // bierzemy pierwszy rekord
                    recentlyChangedTape = false;
                }

                // w pierwszym momencie wykinania while poprzedni rekord zawsze bedzie null
                if (previousRecord == null)
                {
                    AddToTape(AorB, tapeA, tapeB, record);
                    recentlyChangedTape = false;
                }
                else
                {
                    var compare = record.CompareTime(previousRecord);
                    if (compare >= 0)
                    {
                        // seria jest kontynuwoana na danej taśmie
                        //Console.WriteLine($"Later record: {record.GetValue()}");
                        AddToTape(AorB, tapeA, tapeB, record);
                        if (recentlyChangedTape)
                        {
                            seriesCount--;
                            // if (AorB) tapeA.SetSeriesCount(seriesCount);
                            // else tapeB.SetSeriesCount(seriesCount);
                        }
                        recentlyChangedTape = false;
                    }
                    // jezeli jeden jest mniejszy od poprzedniego np. rec = 05:09, prevRec = 13:28
                    // oraz kiedy ostatnio nie zmieniano tasmy
                    else if (compare < 0 && !recentlyChangedTape)
                    {
                        // seria na danej taśmie nie jest kontynuowana
                        //Console.WriteLine($"Earlier record: {record.GetValue()}");

                        seriesCount++; // liczba serii na danej tasmie wzrosła o 1
                        if (AorB)
                        {
                            tapeA.SetSeriesCount(seriesCount);
                            AorB = false; // zmiana na tasme B
                            seriesCount = tapeB.GetSeriesCount();
                            recentlyChangedTape = true;
                        }
                        else
                        {
                            tapeB.SetSeriesCount(seriesCount);
                            AorB = true; // zmiana na tasme A
                            seriesCount = tapeA.GetSeriesCount();
                            recentlyChangedTape = true;
                        }
                        
                    }
                    // jezeli ostatnio zmieniano tasmie to znaczy że jest nowa seria
                    else if (compare < 0 && recentlyChangedTape)
                    {

                        AddToTape(AorB, tapeA, tapeB, record);
                        recentlyChangedTape = false;
                    }
                }
            }
            
            // zapisujemy dane na taśmach pomocniczych 
            tapeA.Flush();
            tapeB.Flush();
            
            // czyscimy tasme z danymi na której w fazie łączenia bedziemy zapisaywać scalane serie z taśm A oraz B
            tapeC.DefaultFileSettings();
            
            _consoleWriter.ShowTapeContent(tapeA);
            _consoleWriter.ShowTapeContent(tapeB);
            _consoleWriter.ShowReadsWritesToDisk(tapeC, tapeB, tapeA);
        }

        private void AddToTape(bool AorB, Tape tapeA, Tape tapeB, Record record)
        {
            if (AorB) tapeA.AddRecord(record);
            else tapeB.AddRecord(record);
        }

        private void Merge(Tape tapeA, Tape tapeB, Tape tapeC)
        {
            // 4. Powtarzamy punkt 3 tak długo, aż serie w którejś z taśm się skończą
            // Jeśli w którejś z taśm jeszcze są serie, kopiujemy to co zostało na koniec taśmy C
            bool endASeries = false;
            bool endBSeries = false;
            
            var recordA = tapeA.GetRecord();
            var recordB = tapeB.GetRecord();
            Record previousRecordA = null;
            Record previousRecordB = null;

            var aWritten = false;
            var bWritten = false;

            var seriesCount = 0;
            
            while (tapeA.CanRead() || tapeB.CanRead())
            {
                // 3. Bierzemy po jednej serii z każdej z taśm (a i b) i scalamy je do taśmy C
                while (!endASeries || !endBSeries)
                {
                    // rozmieszczenie
                    if (!endASeries && !endBSeries)
                    {
                        // patrzymy który rekord jest mniejszy i ten wpisujemy do taśmy C z danymi
                        // np. 10:40 i 12:13 - wynikiem tego porównania wpisujemy 10:40 na taśme C
                        if (recordA.CompareTime(recordB) < 0)
                        {
                            tapeC.AddRecord(recordA);
                            previousRecordA = recordA;
                            aWritten = true;
                        }
                        else
                        {
                            tapeC.AddRecord(recordB);
                            previousRecordB = recordB;
                            bWritten = true;
                        }
                    }

                    if (!endASeries && endBSeries)
                    {
                        tapeC.AddRecord(recordA);
                        previousRecordA = recordA;
                        aWritten = true;
                    }

                    if (endASeries && !endBSeries)
                    {
                        tapeC.AddRecord(recordB);
                        previousRecordB = recordB;
                        bWritten = true;
                    }

                    if (aWritten)
                    {
                        recordA = tapeA.GetRecord();
                        aWritten = false;
                    }

                    if (bWritten)
                    {
                        recordB = tapeB.GetRecord();
                        bWritten = false;
                    }

                    if (recordA == null || previousRecordA != null && recordA.CompareTime(previousRecordA) < 0)
                    {
                        endASeries = true;
                    }

                    if (recordB == null || previousRecordB != null && recordB.CompareTime(previousRecordB) < 0)
                    {
                        endBSeries = true;
                    }
                    
                    if (endASeries && endBSeries) seriesCount++;

                }

                endASeries = false;
                endBSeries = false;
                previousRecordA = null;
                previousRecordB = null;

                if (!tapeA.CanRead())
                {
                    endASeries = true;
                }

                if (!tapeB.CanRead())
                {
                    endBSeries = true;
                }

            }
            // zapisujemy liczbe serii na taśmie C
            tapeC.SetSeriesCount(seriesCount);
            tapeC.Flush();
            
            // czyscimy tasmy pomocnicze
            _tapeA.DefaultFileSettings();
            _tapeB.DefaultFileSettings();
            
            _consoleWriter.ShowTapeContent(tapeC);
            _consoleWriter.ShowReadsWritesToDisk(tapeC, tapeB, tapeA);
        }

        private void Process()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Initial tape content to sort: \n");
            _consoleWriter.ShowTapeContent(_tapeC);
            Console.ResetColor();
            
            // 5. Powtarzamy punkty 2-4 do momentu, gdy w taśmie c będzie tylko jedna seria
            while (_tapeC.GetSeriesCount() != 1)
            {
                _phasesCount++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Phase number {_phasesCount}\n");
                Console.ResetColor();
                
                // 2. Szukamy serii i kopiujemy je na przemian do taśmy A oraz B
                SplitBetweenTapes(_tapeA, _tapeB, _tapeC);

                // faza łączenia
                Merge(_tapeA, _tapeB, _tapeC);
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Total number of phases: {_phasesCount}\n");
            Console.ResetColor();
            
        }

        public void Run(string inputFile)
        {
            DataLoader.LoadData(_tapeC, inputFile); //1. Kopiujemy dane z pliku źródłowego do taśmy z danymi C.
            Process();
        }
    }
}