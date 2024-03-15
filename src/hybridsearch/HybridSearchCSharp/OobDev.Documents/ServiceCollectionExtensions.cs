using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OobDev.Documents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddDocumentServices(this IServiceCollection services)
    {
        services.TryAddTransient<IDocumentConversion, DocumentConversion>();
        services.TryAddTransient<IDocumentConversionChainBuilder, DocumentConversionChainBuilder>();
        services.TryAddTransient<IDocumentConversionHandler, ToTextConversionHandler>();
        return services;
    }
}
