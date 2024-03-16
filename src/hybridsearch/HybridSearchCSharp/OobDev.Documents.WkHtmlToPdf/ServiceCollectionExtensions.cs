using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Documents.Models;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace OobDev.Documents.WkHtmlToPdf;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddWkHtmlToPdfServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentConversionHandler, HtmlToPdfConversionHandler>();
        services.TryAddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));

        services.AddTransient<IDocumentType>(_ => new DocumentType
        {
            Name = "Portable Document Format",
            FileHeader = [(byte)'%', (byte)'P', (byte)'D', (byte)'F', (byte)'-'],
            FileExtensions = [".pdf",],
            ContentTypes = ["application/pdf"],
        });

        return services;
    }
}
