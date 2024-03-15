namespace OobDev.Documents.WkHtmlToPdf;

public class HtmlToPdfConversionHandler : IDocumentConversionHandler
{
    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        if (!SupportedSource(sourceContentType)) throw new NotSupportedException($"Source Content Type \"{sourceContentType}\" is not supported");
        if (!SupportedDestination(destinationContentType)) throw new NotSupportedException($"Source Content Type \"{destinationContentType}\" is not supported");

        //using var reader = new StreamReader(source, Encoding.UTF8);
        //using var writer = new StreamWriter(destination, Encoding.UTF8) { AutoFlush = true, };
        //var html = Markdown.ToHtml(await reader.ReadToEndAsync());
        //await writer.WriteAsync(html);
    }

    public string[] Destinations => ["application/pdf"];
    public bool SupportedDestination(string contentType) => Destinations.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));

    public string[] Sources => ["text/html", "text/xhtml", "text/xhtml+xml"];
    public bool SupportedSource(string contentType) => Sources.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));
}
