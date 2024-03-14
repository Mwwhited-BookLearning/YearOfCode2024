using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OobDev.Search.Sbert;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddSbertServices(this IServiceCollection services)
    {
        //string hostName, int port = 5080

        services.TryAddTransient(sp => ActivatorUtilities.CreateInstance<SBertClient>(sp, ""));
        services.TryAddTransient<IEmbeddingProvider, SentenceEmbeddingProvider>();

        return services;
    }
}
