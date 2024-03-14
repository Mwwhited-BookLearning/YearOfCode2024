using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using OobDev.Search.Providers;
using Qdrant.Client.Grpc;

namespace OobDev.Search.Qdrant;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddOpenSearchServices(this IServiceCollection services)
    {
        services.TryAddTransient<QdrantGrpcClientFactory>();
        services.TryAddTransient(sp => sp.GetRequiredService<QdrantGrpcClientFactory>().Build(""));

        services.TryAddTransient<IPointStructFactory, PointStructFactory>();

        services.TryAddTransient<SemanticStoreProvider>();
        services.TryAddTransient<IStoreContent, SemanticStoreProvider>();
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>, SemanticStoreProvider>(SearchTypes.Semantic);
        services.TryAddTransient<ISearchContent<ScoredPoint>, SemanticStoreProvider>();

        return services;
    }
}
