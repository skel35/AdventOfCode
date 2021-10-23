using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

namespace AdventOfCode;

public abstract partial class Solution
{
    readonly int _day;
    readonly int _year;
    readonly string _inputFile;

    protected Solution(int day, int year)
    {
        _day = day;
        _year = year;
        _inputFile = Path.Combine("..","..","..","..","input", $"{year}_{day:D2}.txt");
    }

    protected string[] ReadLines() => File.ReadAllLines(_inputFile);
    protected string ReadText() => File.ReadAllText(_inputFile);
    protected abstract void Solve();

    public void Run()
    {
        CreateInputFile();
        var sw = Stopwatch.StartNew();
        Solve();
        sw.Stop();
        Console.WriteLine("-----------------");
        Console.WriteLine(sw.ElapsedMilliseconds + " ms");
    }

    void CreateInputFile()
    {
        if (!File.Exists(_inputFile))
        {
            using (File.Create(_inputFile)){}
            DownloadInputFile();
        }
        else if (new FileInfo(_inputFile).Length == 0)
        {
            DownloadInputFile();
        }
    }

    private void DownloadInputFile()
    {
        var url = $@"https://adventofcode.com/{_year}/day/{_day}/input";
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
            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                var str = sr.ReadToEnd().TrimEnd();
                File.WriteAllText(_inputFile, str);
            }
        }
        catch (Exception)
        {
            // Console.WriteLine(e.ToString());
        }
    }
}