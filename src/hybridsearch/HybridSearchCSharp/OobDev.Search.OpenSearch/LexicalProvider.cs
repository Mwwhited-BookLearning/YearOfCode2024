using OobDev.Search.Models;
using OpenSearch.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SearchTypes = OobDev.Search.Models.SearchTypes;

namespace OobDev.Search.OpenSearch;

public class LexicalProvider :
    IStoreContent,
    ISearchContent<SearchResultModel>,
    ISearchContent<JsonNode>
{
    private readonly IOpenSearchLowLevelClient _opensearch;
    private readonly string _storeName;

    public LexicalProvider(
        string hostName,
        string userName, string password,
        string storeName,
        int port = 9200
        )
    {
        _storeName = storeName;

        var lexicalFactory = new OpenSearchClientFactory();
        _opensearch = lexicalFactory.GetClient(hostName, userName, password, port);
        Console.WriteLine($"connect to full-text");
    }

    public async IAsyncEnumerable<SearchResultModel> QueryAsync(string? queryString, int limit = 25, int page = 0)
    {
        await foreach (var item in ((ISearchContent<JsonNode>)this).QueryAsync(queryString, 25, 0))
            yield return new()
            {
                Score = (float?)item["_score"] ?? 0,
                PathHash = (item["_source"]?[nameof(SearchResultModel.PathHash)]?.ToString()) ?? "",
                Content = (item["_source"]?[nameof(SearchResultModel.Content)]?.ToString()) ?? "",
                File = (item["_source"]?[nameof(SearchResultModel.File)]?.ToString()) ?? "",
                Type = SearchTypes.Lexical,
            };
    }

    async IAsyncEnumerable<JsonNode> ISearchContent<JsonNode>.QueryAsync(string? queryString, int limit, int page)
    {
        if (string.IsNullOrWhiteSpace(queryString))
            yield break;

        // https://opensearch.org/docs/latest/query-dsl/full-text/index/
        var lookupResult = await _opensearch.SearchAsync<StringResponse>(_storeName,
           PostData.Serializable(new
           {
               query = new
               {
                   match = new
                   {
                       Content = queryString
                   }
               }
           }));

        var lookupJson = JsonNode.Parse(lookupResult.Body);
        if (lookupJson?["hits"]?["hits"] is JsonArray arr)
        {
            foreach (var item in arr)
                if (item != null)
                    yield return item;
        }
    }

    public async Task<bool> TryStoreAsync(string full, string file, string pathHash)
    {
        var lookupResult = await _opensearch.SearchAsync<StringResponse>(_storeName,
           PostData.Serializable(new
           {
               query = new
               {
                   match = new
                   {
                       PathHash = new
                       {
                           query = pathHash
                       }
                   }
               }
           }));

        var lookupJson = JsonNode.Parse(lookupResult.Body);
        if (lookupJson?["hits"]?["hits"] is JsonArray arr && arr.Count > 0)
            return false;

        Console.WriteLine($"store -> {file}");

        var id = Guid.NewGuid().ToString();
        var result = await _opensearch.IndexAsync<StringResponse>(_storeName, id, PostData.Serializable(new
        {
            Id = id,

            File = file,
            OriginalFile = full,
            PathHash = pathHash,

            FileName = Path.GetFileNameWithoutExtension(file),
            Directory = Path.GetDirectoryName(file) ?? "",
            Extensions = Path.GetExtension(file),

            Content = File.ReadAllText(full),
        }));

        return true;
    }
}
