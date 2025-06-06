using System.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
public class ZenConfigParser
{
    public string results;
    public int Width;
    public int Height;
    public string Title;
    public string whatdoesthisstringdo;
    public ZenConfigParser(string FILEPATHANDNAME)
    {
        results = File.ReadAllText(FILEPATHANDNAME);
        Console.WriteLine(results);
        string[] splitparts = results.Split(",");
        foreach (var part in splitparts)
        {
            var kv = part.Split('=');
            if (kv.Length == 2)
            {
                var key = kv[0].Trim();
                // Remove "nl:" prefix if present
                if (key.StartsWith("nl:", StringComparison.OrdinalIgnoreCase))
                {
                    key = key.Substring(3);
                }
                var value = kv[1].Trim();
                if (key.Equals("Width", StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(value, out Width);
                }
                else if (key.Equals("Height", StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(value, out Height);
                }
                else if (key.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    // Remove surrounding quotes if present
                    Title = value.Trim('"');
                }
                else if (key.Equals("amongus", StringComparison.OrdinalIgnoreCase));
                {
                    whatdoesthisstringdo = value.Trim('"');
                }
            }
        }
    }
}