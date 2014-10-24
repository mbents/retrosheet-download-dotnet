using System;
using System.IO;

namespace RetrosheetEventDataParser
{
    public class BGAME
    {
        private string _extractBaseDir, _destinationDir;

        public BGAME(string baseDir, string destinationDir)
        {
            _extractBaseDir = baseDir;
            _destinationDir = destinationDir;
        }

        public void Run()
        {
            var eventFiles = new DirectoryInfo(_extractBaseDir).GetFiles("*.EV*");
            foreach (var file in eventFiles)
            {
                Console.WriteLine(string.Format("BGAME parsing file {0}", file.Name));
                var args = string.Format("/c BGAME -y {0} {1} > {3}\\{2}.csv", file.Name.Substring(0, 4), file.Name,
                    file.Name.Substring(0, file.Name.IndexOf(".")), _destinationDir);
                Command.Run(_extractBaseDir, args);
            }
        }
    }
}
