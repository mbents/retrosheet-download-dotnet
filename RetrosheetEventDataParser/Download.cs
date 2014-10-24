using System;
using System.Net;
using System.IO;

namespace RetrosheetEventDataParser
{
    public class Download
    {
        private const string ADDRESS = "http://www.retrosheet.org/events/";

        private string _extractBaseDir;
        private int? _year;

        public Download(string baseDir, int? year)
        {
            _extractBaseDir = baseDir;
            _year = year;
        }

        public void Run()
        {
            Console.WriteLine("Starting event zip file download...");
            Console.WriteLine("============================================");

            if (_year.HasValue)
                DownloadAndExtract(_year.Value);
            else
            {
                for (_year = 1921; _year < DateTime.Now.Year; _year++)
                    DownloadAndExtract(_year.Value);
            }
        }

        private void DownloadAndExtract(int year)
        {
            string fileName = string.Format("{0}eve.zip", year);
            string resourceLocation = string.Format("{0}{1}", ADDRESS, fileName);
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
                System.IO.Compression.ZipFile.ExtractToDirectory(fileName, _extractBaseDir);
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
}
