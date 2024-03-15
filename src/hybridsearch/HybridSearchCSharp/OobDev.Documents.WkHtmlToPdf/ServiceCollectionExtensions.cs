using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace OobDev.Documents.WkHtmlToPdf;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddWkHtmlToPdfServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentConversionHandler, HtmlToPdfConversionHandler>();
        services.TryAddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
        return services;
    }
}
