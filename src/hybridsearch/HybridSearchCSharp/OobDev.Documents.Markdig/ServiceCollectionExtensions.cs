using Microsoft.Extensions.DependencyInjection;

namespace OobDev.Documents.Markdig;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddMarkdigServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentConversionHandler, MarkdownToHtmlConversionHandler>();
        return services;
    }
}
