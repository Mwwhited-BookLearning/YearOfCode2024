using OpenSearch.Net;
using System;

namespace OobDev.Search.OpenSearch;

public class OpenSearchClientFactory
{
    public IOpenSearchLowLevelClient GetClient(
        string hostName,
        string username, string password,
        int port = 9200
        )
    {
        var connection = new ConnectionConfiguration(
                new Uri($"http://{hostName}:{port}")
            )
            .BasicAuthentication(username, password)
            .EnableHttpCompression(true)
            .ThrowExceptions(true)
            //.PrettyJson()
            ;
        var client = new OpenSearchLowLevelClient(connection);
        return client;
    }
}
