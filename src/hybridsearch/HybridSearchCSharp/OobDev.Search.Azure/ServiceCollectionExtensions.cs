using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OobDev.Search.Models;
using OobDev.Search.Providers;

namespace OobDev.Search.Azure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddAzureServices(this IServiceCollection services)
    {
        services.TryAddTransient<BlobProvider>();
        services.TryAddTransient<IStoreContent, BlobProvider>();
        services.TryAddTransient<ISearchContent<BlobItem>, BlobProvider>();
        services.TryAddTransient<IGetContent<ContentReference>, BlobProvider>();
        return services;
    }
}
