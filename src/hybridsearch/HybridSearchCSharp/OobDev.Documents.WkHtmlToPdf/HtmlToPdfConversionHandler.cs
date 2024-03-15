using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace OobDev.Documents.WkHtmlToPdf;

public class HtmlToPdfConversionHandler : IDocumentConversionHandler
{
    private readonly IConverter _converter;

    public HtmlToPdfConversionHandler(IConverter converter)
    {
        _converter = converter;
    }

    public async Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType)
    {
        if (!SupportedSource(sourceContentType)) throw new NotSupportedException($"Source Content Type \"{sourceContentType}\" is not supported");
        if (!SupportedDestination(destinationContentType)) throw new NotSupportedException($"Source Content Type \"{destinationContentType}\" is not supported");

        using var reader = new StreamReader(source, leaveOpen: true);
        var html = await reader.ReadToEndAsync();

        //TODO: change this to config stuff
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
            },
            Objects = {
                new () {
                    PagesCount = true,
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                }
            }
        };
        var pdf = _converter.Convert(doc); 
        await destination.WriteAsync(pdf);
    }

    public string[] Destinations => ["application/pdf"];
    public bool SupportedDestination(string contentType) => Destinations.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));

    public string[] Sources => ["text/html", "text/xhtml", "text/xhtml+xml"];
    public bool SupportedSource(string contentType) => Sources.Any(t => string.Equals(t, contentType, StringComparison.OrdinalIgnoreCase));

}
