using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Documents;

public class ToTextConversionHandler : IDocumentConversionHandler
{
    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        if (!SupportedSource(sourceContentType)) throw new NotSupportedException($"Source Content Type \"{sourceContentType}\" is not supported");
        if (!SupportedDestination(destinationContentType)) throw new NotSupportedException($"Source Content Type \"{destinationContentType}\" is not supported");

        await source.CopyToAsync(destination);
    }

    public string[] Destinations => ["text/plain"];
    public bool SupportedDestination(string contentType) => Destinations.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));

    public string[] Sources => ["application/octet-stream"];
    public bool SupportedSource(string contentType) => true; // Sources.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));
}
