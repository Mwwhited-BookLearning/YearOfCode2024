using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OobDev.Search.Sbert;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddSbertServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SBertOptions>(options => configuration.Bind(nameof(SBertOptions), options));
        services.TryAddTransient<SBertClient>();
        services.TryAddTransient<IEmbeddingProvider, SentenceEmbeddingProvider>();

        return services;
    }
}
