using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OobDev.Search.Providers;
using System;

namespace OobDev.Search.Qdrant;

public class SemanticStoreProviderFactory : ISemanticStoreProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<QdrantOptions> _options;

    public SemanticStoreProviderFactory(
        IServiceProvider serviceProvider,
        IOptions<QdrantOptions> options
        )
    {
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public SemanticStoreProvider Create(bool forSummary) =>
        ActivatorUtilities.CreateInstance<SemanticStoreProvider>(_serviceProvider, _options.Value.CollectionName, forSummary);
}
