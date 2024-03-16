using Microsoft.Extensions.DependencyInjection;
using OobDev.Documents.Models;

namespace OobDev.Documents.Markdig;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddMarkdigServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentConversionHandler, MarkdownToHtmlConversionHandler>();

        services.AddTransient<IDocumentType>(_ => new DocumentType
        {
            Name = "Markdown",
            FileHeader = [],
            FileExtensions = [".md",],
            ContentTypes = ["text/markdown", "text/x-markdown"],
        });

        return services;
    }
}
