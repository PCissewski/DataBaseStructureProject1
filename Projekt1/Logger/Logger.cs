namespace Projekt1;

public class Logger : ILogger
{
    private const string InputTapeContentCopy =
        "X:\\Studia\\InformatykaSemestr5\\SBD\\Project1\\Projekt1\\Projekt1\\Logger\\inputTapeContent.txt";
    public void SaveInputTapeContent(string sourceFile)
    {
        File.Delete(InputTapeContentCopy);
        File.Copy(sourceFile, InputTapeContentCopy);
    }
}