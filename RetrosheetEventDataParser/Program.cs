using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetrosheetEventDataParser
{
    class Program
    {
        private static bool DownloadEventFiles = false;
        private static bool ParseBEVENT = false;
        private static bool ParseBGAME = false;
        private static string WorkingDirectory;
        private static string DestinationDirectory;
        private static int? Year;

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                    case "-help":
                        ShowHelp();
                        break;
                    case "-d":
                    case "-download":
                        DownloadEventFiles = true;
                        break;
                    case "-bevent":
                        ParseBEVENT = true;
                        break;
                    case "-bgame":
                        ParseBGAME = true;
                        break;
                    case "-wd":
                        WorkingDirectory = args[i + 1];
                        break;
                    case "-dir":
                        DestinationDirectory = args[i + 1];
                        break;
                    case "-y":
                        Year = Convert.ToInt32(args[i + 1]);
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(WorkingDirectory))
            {
                Console.WriteLine("-wd option is required. Please run again with working directory set.");
            }
            else
            {
                if (string.IsNullOrEmpty(DestinationDirectory)) DestinationDirectory = WorkingDirectory;

                var start = DateTime.Now;

                if (DownloadEventFiles)
                    new Download(WorkingDirectory, Year).Run();

                var taskList = new List<Task>();
                if (ParseBEVENT)
                {
                    var task = new Task(() => new BEVENT(WorkingDirectory).Run());
                    task.Start();
                    taskList.Add(task);
                }

                if (ParseBGAME)
                {
                    var task = new Task(() => new BGAME(WorkingDirectory, DestinationDirectory).Run());
                    task.Start();
                    taskList.Add(task);
                }

                if (taskList.Count > 0)
                    Task.WaitAll(taskList.ToArray());

                Console.WriteLine("============================================");
                Console.WriteLine(string.Format("Finished {0}parsing data in {1}", 
                    DownloadEventFiles ? "downloading files and " : "",
                    (DateTime.Now - start)));
            }

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        private static void ShowHelp()
        {
            var helpMessage = new StringBuilder();

            helpMessage.AppendLine("Usage: RetrosheetEventDataParser [options] directory");
            helpMessage.AppendLine("options:");
            helpMessage.AppendLine(" -h or -help         show this help screen");
            helpMessage.AppendLine(" -d or -download     download event files");
            helpMessage.AppendLine(" -bevent             parse event files using BEVENT");
            helpMessage.AppendLine(" -bgame              parse event files using BGAME");
            helpMessage.AppendLine(" -wd                 directory where BEVENT/BGAME and event files are located");
            helpMessage.AppendLine(" -dir                directory for parsed files; default is working directory");
            helpMessage.AppendLine(" -y                  specific year for which event files should be downloaded");

            Console.WriteLine(helpMessage.ToString());
        }

    }
}
