using OobDev.Search.Models;
using System.Collections.Generic;
using System.IO;

namespace OobDev.Search.IO;

public static class FileTools
{
    public const int DefaultContextLength = 4096;
    public const int DefaultOverlap = 0;

    public static async IAsyncEnumerable<ContentChunk> SplitFileAsync(
        string filename,
        int contextLength = DefaultContextLength,
        int overlap = DefaultOverlap
        )
    {
        using var file = File.OpenRead(filename);
        using var reader = new StreamReader(file);

        var buffer = new char[contextLength];

        int sequence = 0;
        while (file.Position < file.Length)
        {
            var start = file.Position;
            var read = await reader.ReadBlockAsync(buffer, 0, contextLength);
            if (read > 0)
                yield return new(new string(buffer, 0, read), sequence, start, read);
            else if (read < contextLength)
                break;
            file.Position -= overlap;
            sequence++;
        }
    }
}
