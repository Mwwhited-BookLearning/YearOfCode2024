using Microsoft.Extensions.Options;
using OpenSearch.Net;
using System;

namespace OobDev.Search.OpenSearch;

public class OpenSearchClientFactory : IOpenSearchClientFactory
{
    private readonly IOptions<OpenSearchOptions> _config;

    public OpenSearchClientFactory(
        IOptions<OpenSearchOptions> config
        )
    {
        _config = config;
    }

    public IOpenSearchLowLevelClient Create()
    {
        var connection = new ConnectionConfiguration(
                new Uri($"http://{_config.Value.HostName}:{_config.Value.Port}")
            )
            .EnableHttpCompression(true)
            .ThrowExceptions(true)
            ;

        if (!string.IsNullOrWhiteSpace(_config.Value.UserName) && !string.IsNullOrWhiteSpace(_config.Value.Password))
        {
            connection.BasicAuthentication(_config.Value.UserName, _config.Value.Password);
        }

        var client = new OpenSearchLowLevelClient(connection);
        return client;
    }
}
