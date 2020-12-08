using System;
using System.IO;
using System.Net;
using System.Text;
using AdventOfCode;

namespace FetchInput
{
    partial class Program
    {
        //private const string SessionId = "your-session-id-from-cookies";
        private const bool Autodetect = true;
        private const int Day = 3;
        private const int Year = 2020;

        static void Main(string[] args)
        {
            int day;
            int year;

            if (Autodetect)
            {
                var nowUtcMinusFive = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5));
                day = nowUtcMinusFive.Day;
                year = nowUtcMinusFive.Year;
            }
            else
            {
                day = Day;
                year = Year;
            }

            var inputFile = Path.Combine("..","..","..","..","input", $"{year}_{day:D2}.txt");
            DownloadInputFile(inputFile, day, year);
        }

        private static void DownloadInputFile(string inputFile, int day, int year)
        {
            var url = $@"https://adventofcode.com/{year}/day/{day}/input";
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                const string accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                accept.Split(',').ForEach(h => req.Headers.Add(HttpRequestHeader.Accept, h));
                if (req.CookieContainer == null) req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(new Cookie("session", SessionId, "/", ".adventofcode.com"));
                var response = req.GetResponse();
                using var sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                var str = sr.ReadToEnd().TrimEnd();
                File.WriteAllText(inputFile, str);
            }
            catch (Exception)
            {
                // Console.WriteLine(e.ToString());
            }
        }

    }
}