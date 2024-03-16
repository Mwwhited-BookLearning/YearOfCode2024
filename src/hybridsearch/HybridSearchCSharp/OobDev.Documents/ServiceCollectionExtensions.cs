using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Documents.Models;

namespace OobDev.Documents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddDocumentServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentConversion, DocumentConversion>();
        services.TryAddTransient<IDocumentConversionChainBuilder, DocumentConversionChainBuilder>();
        services.TryAddTransient<IDocumentConversionHandler, ToTextConversionHandler>();

        services.AddTransient<IDocumentType>(_ => new DocumentType
        {
            Name = "HyperText Markup Language",
            FileHeader = [],
            FileExtensions = [".html", ".htm", ".xhtml", ".xhtm"],
            ContentTypes = ["text/html", "text/xhtml", "text/xhtml+xml"],
        });
        services.AddTransient<IDocumentType>(_ => new DocumentType
        {
            Name = "Plain Text",
            FileHeader = [],
            FileExtensions = [".txt", ".text"],
            ContentTypes = ["text/plain"],
        });
        services.AddTransient<IDocumentType>(_ => new DocumentType
        {
            Name = "Extensible Markup Language",
            FileHeader = [],
            FileExtensions = [".xml"],
            ContentTypes = ["text/xml"],
        });
        return services;
    }
}
