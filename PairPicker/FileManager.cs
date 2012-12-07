using System.IO;

namespace PairPicker
{
    public class FileManager
    {
        public string[] LoadFile(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteFile(string path, string[] lines)
        {
            File.WriteAllLines(path, lines);
        }
    }
}
