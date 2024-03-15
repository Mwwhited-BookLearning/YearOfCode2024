using Microsoft.Extensions.DependencyInjection;

namespace OobDev.Documents.WkHtmlToPdf;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddPdfServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentConversionHandler, HtmlToPdfConversionHandler>();
        return services;
    }
}
