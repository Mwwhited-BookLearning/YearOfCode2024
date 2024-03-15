using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(sourceContentType, nameof(sourceContentType));
        ArgumentNullException.ThrowIfNull(destination, nameof(destination));
        ArgumentNullException.ThrowIfNull(destinationContentType, nameof(destinationContentType));

        if (string.Equals(sourceContentType, destinationContentType, StringComparison.OrdinalIgnoreCase))
            await source.CopyToAsync(destination);

        var steps = _chain.Steps(sourceContentType, destinationContentType);
        if (steps.Length == 0) throw new NotSupportedException($"Conversion from \"{sourceContentType}\" to \"{destinationContentType}\" is not supported");
        else if (steps.Length == 1) await steps[0].Handler.ConvertAsync(source, sourceContentType, destination, destinationContentType);



        //TODO: build change
        throw new NotSupportedException();
    }
}
public class DocumentConversionChainBuilder : IDocumentConversionChainBuilder
{
    private readonly IEnumerable<IDocumentConversionHandler> _handlers;

    public DocumentConversionChainBuilder(IEnumerable<IDocumentConversionHandler> handlers)
    {
        _handlers = handlers;
    }

    public ChainStep[] Steps(string sourceContentType, string destinationContentType)
    {
        var simple = _handlers.FirstOrDefault(h => h.SupportedSource(sourceContentType) && h.SupportedDestination(destinationContentType));
        if (simple != null) return [new ChainStep() { Handler = simple, SourceContentType = sourceContentType, DestinationContentType = destinationContentType }];


        //TODO: build graph

        //        _handlers.Where(s=>s.SupportedSource(sourceContentType)).ToList();

        //var steps = new List<ChainStep>();
        //do
        //{
        //} while (steps[^1].DestinationContentType != destinationContentType);
        //_handlers.Where()

        throw new NotImplementedException();
    }
}

