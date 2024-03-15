using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using OobDev.Search.Providers;

namespace OobDev.Search.Azure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddAzureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureBlobProviderOptions>(options => configuration.Bind(nameof(AzureBlobProviderOptions), options));
        services.TryAddTransient<IBlobServiceClientFactory, BlobServiceClientFactory>();
        services.TryAddTransient(sp => sp.GetRequiredService<IBlobServiceClientFactory>().Create());
        services.TryAddTransient<IBlobProviderFactory, BlobProviderFactory>();
        services.AddKeyedTransient(BlobProviderFactory.DocumentCollectionKey, (sp, k) => sp.GetRequiredService<IBlobProviderFactory>().Create(k as string));
        services.AddKeyedTransient(BlobProviderFactory.SummaryCollectionKey, (sp, k) => sp.GetRequiredService<IBlobProviderFactory>().Create(k as string));
        services.TryAddTransient<IStoreContent>(sp => sp.GetRequiredKeyedService<BlobProvider>(BlobProviderFactory.DocumentCollectionKey));
        services.TryAddTransient<ISearchContent<SearchResultModel>>(sp => sp.GetRequiredKeyedService<BlobProvider>(BlobProviderFactory.DocumentCollectionKey));
        services.TryAddKeyedTransient<ISearchContent<SearchResultModel>>(SearchTypes.None, (sp,k) => sp.GetRequiredKeyedService<BlobProvider>(BlobProviderFactory.DocumentCollectionKey));
        services.TryAddTransient<IGetContent<ContentReference>>(sp => sp.GetRequiredKeyedService<BlobProvider>(BlobProviderFactory.DocumentCollectionKey));
        services.TryAddTransient<IGetSummary<ContentReference>>(sp => sp.GetRequiredKeyedService<BlobProvider>(BlobProviderFactory.SummaryCollectionKey));
        return services;
    }
}
