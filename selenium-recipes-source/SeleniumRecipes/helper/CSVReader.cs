using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CSVReader
{
    private static bool ignoreFirstLineDefault = false;

    public static IEnumerable<IList<string>> FromFile(string fileName)
    {
        foreach (IList<string> item in FromFile(fileName, ignoreFirstLineDefault)) yield return item;
    }

    public static IEnumerable<IList<string>> FromFile(string fileName, bool ignoreFirstLine)
    {
        using (StreamReader rdr = new StreamReader(fileName))
        {
            foreach (IList<string> item in FromReader(rdr, ignoreFirstLine)) yield return item;
        }
    }

    public static IEnumerable<IList<string>> FromStream(Stream csv)
    {
        foreach (IList<string> item in FromStream(csv, ignoreFirstLineDefault)) yield return item;
    }

    public static IEnumerable<IList<string>> FromStream(Stream csv, bool ignoreFirstLine)
    {
        using (var rdr = new StreamReader(csv))
        {
            foreach (IList<string> item in FromReader(rdr, ignoreFirstLine)) yield return item;
        }
    }

    public static IEnumerable<IList<string>> FromReader(TextReader csv)
    {
        //Probably should have used TextReader instead of StreamReader
        foreach (IList<string> item in FromReader(csv, ignoreFirstLineDefault)) yield return item;
    }

    public static IEnumerable<IList<string>> FromReader(TextReader csv, bool ignoreFirstLine)
    {
        if (ignoreFirstLine) csv.ReadLine();

        IList<string> result = new List<string>();

        StringBuilder curValue = new StringBuilder();
        char c;
        c = (char)csv.Read();
        while (csv.Peek() != -1)
        {
            switch (c)
            {
                case ',': //empty field
                    result.Add("");
                    c = (char)csv.Read();
                    break;
                case '"': //qualified text
                case '\'':
                    char q = c;
                    c = (char)csv.Read();
                    bool inQuotes = true;
                    while (inQuotes && csv.Peek() != -1)
                    {
                        if (c == q)
                        {
                            c = (char)csv.Read();
                            if (c != q)
                                inQuotes = false;
                        }

                        if (inQuotes)
                        {
                            curValue.Append(c);
                            c = (char)csv.Read();
                        }
                    }
                    result.Add(curValue.ToString());
                    curValue = new StringBuilder();
                    if (c == ',') c = (char)csv.Read(); // either ',', newline, or endofstream
                    break;
                case '\n': //end of the record
                case '\r':
                    //potential bug here depending on what your line breaks look like
                    if (result.Count > 0) // don't return empty records
                    {
                        yield return result;
                        result = new List<string>();
                    }
                    c = (char)csv.Read();
                    break;
                default: //normal unqualified text
                    while (c != ',' && c != '\r' && c != '\n' && csv.Peek() != -1)
                    {
                        curValue.Append(c);
                        c = (char)csv.Read();
                    }
                    result.Add(curValue.ToString());
                    curValue = new StringBuilder();
                    if (c == ',') c = (char)csv.Read(); //either ',', newline, or endofstream
                    break;
            }

        }
        if (curValue.Length > 0) //potential bug: I don't want to skip on a empty column in the last record if a caller really expects it to be there
            result.Add(curValue.ToString());
        if (result.Count > 0)
            yield return result;

    }
}