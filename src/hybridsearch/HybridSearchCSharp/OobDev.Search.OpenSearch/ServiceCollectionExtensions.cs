using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using System.Text.Json.Nodes;

namespace OobDev.Search.OpenSearch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddOpenSearchServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenSearchOptions>(options => configuration.Bind(nameof(OpenSearchOptions), options));
        services.Configure<OpenSearchOptions>(nameof(OpenSearchOptions), opt => { });
        services.TryAddTransient<IOpenSearchClientFactory, OpenSearchClientFactory>();
        services.TryAddTransient(sp => sp.GetRequiredService<IOpenSearchClientFactory>().Create());
        services.TryAddTransient<LexicalProvider>();
        services.TryAddTransient<IStoreContent, LexicalProvider>();
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>, LexicalProvider>(SearchTypes.Lexical);
        services.TryAddTransient<ISearchContent<JsonNode>, LexicalProvider>();

        return services;
    }
}
