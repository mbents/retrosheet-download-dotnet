using System;
using System.IO;

namespace RetrosheetEventDataParser
{
    public class BEVENT
    {
        private string _extractBaseDir;

        public BEVENT(string baseDir)
        {
            _extractBaseDir = baseDir;
        }

        public void Run()
        {
            var eventFiles = new DirectoryInfo(_extractBaseDir).GetFiles("*.EV*");
            foreach (var file in eventFiles)
            {
                Console.WriteLine(string.Format("BEVENT parsing file {0}", file.Name));
                var args = string.Format("/c BEVENT -y {0} -f 0-96 {1} > {2}.csv", file.Name.Substring(0, 4), file.Name,
                    file.Name.Substring(0, file.Name.IndexOf(".")));
                Command.Run(_extractBaseDir, args);
            }
        }
    }
}
