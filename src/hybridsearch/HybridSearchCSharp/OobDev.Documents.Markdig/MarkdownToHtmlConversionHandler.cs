using Markdig;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Documents.Markdig;

public class MarkdownToHtmlConversionHandler : IDocumentConversionHandler
{
    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        if (!SupportedSource(sourceContentType)) throw new NotSupportedException($"Source Content Type \"{sourceContentType}\" is not supported");
        if (!SupportedDestination(destinationContentType)) throw new NotSupportedException($"Source Content Type \"{destinationContentType}\" is not supported");

        using var reader = new StreamReader(source, leaveOpen: true);
        using var writer = new StreamWriter(destination, leaveOpen: true) { AutoFlush = true, };
        var html = Markdown.ToHtml(await reader.ReadToEndAsync());
        await writer.WriteAsync(html);
    }

    public string[] Destinations => ["text/html", "text/xhtml", "text/xhtml+xml"];
    public bool SupportedDestination(string contentType) => Destinations.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));
    public string[] Sources => ["text/markdown", "text/x-markdown", "text/plain"];
    public bool SupportedSource(string contentType) => Sources.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));
}
