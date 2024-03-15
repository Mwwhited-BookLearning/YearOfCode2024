using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using OobDev.Search.Providers;
using Qdrant.Client.Grpc;

namespace OobDev.Search.Qdrant;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddQdrantServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QdrantOptions>(options => configuration.Bind(nameof(QdrantOptions), options));
        services.TryAddTransient<IQdrantGrpcClientFactory, QdrantGrpcClientFactory>();
        services.TryAddTransient(sp => sp.GetRequiredService<IQdrantGrpcClientFactory>().Create());
        services.TryAddTransient<ISemanticStoreProviderFactory, SemanticStoreProviderFactory>();

        services.TryAddTransient<SemanticStoreProvider>(sp => sp.GetRequiredService<ISemanticStoreProviderFactory>().Create(false));

        services.TryAddTransient<IPointStructFactory, PointStructFactory>();
        services.TryAddTransient<IStoreContent>(sp => sp.GetRequiredService<SemanticStoreProvider>());
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>>(SearchTypes.Semantic, (sp, k) => sp.GetRequiredService<SemanticStoreProvider>());
        services.TryAddTransient<ISearchContent<ScoredPoint>>(sp => sp.GetRequiredService<SemanticStoreProvider>());

        return services;
    }
}
