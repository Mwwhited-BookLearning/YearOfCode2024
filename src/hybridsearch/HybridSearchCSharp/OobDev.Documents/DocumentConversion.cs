using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OobDev.Documents;

public class DocumentConversion : IDocumentConversion
{
    private readonly IDocumentConversionChainBuilder _chain;

    public DocumentConversion(IDocumentConversionChainBuilder chain)
    {
        _chain = chain;
    }

    private readonly Dictionary<(string source, string destination), ChainStep[]> _cache = [];

    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourceContentType, nameof(sourceContentType));
        ArgumentNullException.ThrowIfNull(destination, nameof(destination));
        ArgumentNullException.ThrowIfNull(destinationContentType, nameof(destinationContentType));

        if (string.Equals(sourceContentType, destinationContentType, StringComparison.OrdinalIgnoreCase))
            await source.CopyToAsync(destination);

        ChainStep[] steps;
        if (_cache.TryGetValue((sourceContentType, destinationContentType), out var cached))
        {
            steps = cached;
        }
        else
        {
            steps = _chain.Steps(sourceContentType, destinationContentType);
            _cache.TryAdd((sourceContentType, destinationContentType), steps);
        }

        if (steps.Length == 0) throw new NotSupportedException($"Conversion from \"{sourceContentType}\" to \"{destinationContentType}\" is not supported");
        else if (steps.Length == 1) await steps[0].Handler.ConvertAsync(source, sourceContentType, destination, destinationContentType);
        else
        {
            MemoryStream? temp = null;
            foreach (var step in steps)
            {
                temp = new MemoryStream();
                await step.Handler.ConvertAsync(source, step.SourceContentType, temp, step.DestinationContentType);
                temp.Position = 0;
                source = temp;
            }
            await (temp ?? source).CopyToAsync(destination);
            destination.Position = 0;
        }
    }
}

