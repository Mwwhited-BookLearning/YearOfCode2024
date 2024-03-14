using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using System.Text.Json.Nodes;

namespace OobDev.Search.OpenSearch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddOpenSearchServices(this IServiceCollection services)
    {
        services.TryAddTransient(sp=>ActivatorUtilities.CreateInstance<LexicalProvider>(sp, "")));
        services.TryAddTransient<IStoreContent, LexicalProvider>();
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>, LexicalProvider>(SearchTypes.Lexical);
        services.TryAddTransient<ISearchContent<JsonNode>, LexicalProvider>();

        return services;
    }
}
