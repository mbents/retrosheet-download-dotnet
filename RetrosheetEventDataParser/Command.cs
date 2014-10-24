using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RetrosheetEventDataParser
{
    public class Command
    {
        public static void Run(string workingDirectory, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.FileName = "CMD.EXE";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = arguments;

            using (var p = Process.Start(startInfo))
            {
                p.WaitForExit();
            }
        }
    }
}
