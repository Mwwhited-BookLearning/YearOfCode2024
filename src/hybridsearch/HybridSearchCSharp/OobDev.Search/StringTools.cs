using System;
using System.Collections.Generic;
using System.IO;

namespace OobDev.Search;

public static class StringTools
{
    public static IEnumerable<string> SplitBy(this string input, int length = 80, char breaker = ' ')
    {
        var reader = new StringReader(input);

        while (true)
        {
            var line = reader.ReadLine();
            if (line == null)
                break;

            if (line.Length > length)
            {
                var nextStart = 0;

                while (nextStart < line.Length)
                {
                    if (nextStart + length > line.Length)
                    {
                        var chunk = line[nextStart..];
                        yield return chunk.TrimEnd('\r', '\n', ' ');
                        break;
                    }
                    else
                    {
                        var chunk = line[nextStart..(nextStart + length)];
                        if (chunk == null)
                            break;
                        else if (chunk.Length < length)
                        {
                            yield return chunk.TrimEnd('\r', '\n', ' ');
                            nextStart += length;
                        }

                        var last = chunk.LastIndexOf(breaker);

                        if (last > 0)
                        {
                            chunk = chunk[..last];
                            yield return chunk.TrimEnd('\r', '\n', ' ');
                            nextStart += chunk.Length + 1;
                        }
                        else if (last == 0)
                        {
                            nextStart++;
                        }
                        else
                        {
                            yield return chunk.TrimEnd('\r', '\n', ' ');
                            nextStart += chunk.Length;
                        }
                    }
                }
            }
            else
            {
                yield return line.TrimEnd('\r', '\n', ' ');
            }
        }
    }

    public static string WriteAsLines(this IEnumerable<string> lines) => string.Join(Environment.NewLine, lines);
}
