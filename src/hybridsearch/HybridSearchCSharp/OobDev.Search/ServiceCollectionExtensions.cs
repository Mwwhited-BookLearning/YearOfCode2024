using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using OobDev.Search.Providers;

namespace OobDev.Search;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddSearchServices(this IServiceCollection services)
    {
        services.TryAddTransient<ISummerizeContent, DocumentSummaryGenerationProvider>();
        services.TryAddTransient<ISearchContent<SearchResultModel>, HybridProvider>();
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>, HybridProvider>(SearchTypes.Hybrid);

        services.TryAddTransient<ISearchProvider, SearchProvider>();

        return services;
    }
}
