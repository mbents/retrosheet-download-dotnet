using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RetrosheetEventDataParser
{
    class Program
    {
        private const string EXTRACT_BASE_DIR = "C:\\retrosheet";

        static void Main(string[] args)
        {
            var start = DateTime.Now;
            DownloadAndExtractData();
            ParseEventFiles();
            Console.WriteLine("============================================");
            Console.WriteLine(string.Format("Finished downloading files and parsing data in {0}", (DateTime.Now - start)));
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        private static void DownloadAndExtractData()
        {
            Console.WriteLine("Starting event zip file download...");
            Console.WriteLine("============================================");
            string address = "http://www.retrosheet.org/events/";
            int year = 1921;
            for (year = 1921; year < DateTime.Now.Year; year++)
            {
                string fileName = string.Format("{0}eve.zip", year);
                string resourceLocation = string.Format("{0}{1}", address, fileName);
                try
                {
                    Console.WriteLine(string.Format("Downloading file {0}", fileName));
                    using (WebClient client = new WebClient())
                    {
                        // wait a little bit so we're not throttling their servers
                        System.Threading.Thread.Sleep(2500);
                        client.DownloadFile(resourceLocation, fileName);
                    }
                    Console.WriteLine(string.Format("Extracting file {0}", fileName));
                    System.IO.Compression.ZipFile.ExtractToDirectory(fileName, EXTRACT_BASE_DIR);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
            }
        }

        private static void ParseEventFiles()
        {
            Console.WriteLine("Starting event file parsing...");
            Console.WriteLine("============================================");
            var eventFiles = new DirectoryInfo(EXTRACT_BASE_DIR).GetFiles("*.EV*");
            foreach (var file in eventFiles)
            {
                Console.WriteLine(string.Format("Parsing file {0}", file.Name));
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = EXTRACT_BASE_DIR;
                startInfo.FileName = "CMD.EXE";
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.Arguments = string.Format("/c BEVENT -y {0} -f 0-96 {1} > {2}.csv", file.Name.Substring(0, 4), file.Name,
                    file.Name.Substring(0, file.Name.IndexOf(".")));

                //startInfo.RedirectStandardError = true;

                using (var p = System.Diagnostics.Process.Start(startInfo))
                {
                    //var err = p.StandardError.ReadToEnd();
                    //Console.WriteLine(err);
                    p.WaitForExit();
                }
            }
        }

    }
}
