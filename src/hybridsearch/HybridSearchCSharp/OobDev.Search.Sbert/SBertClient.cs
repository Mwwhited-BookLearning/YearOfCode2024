using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OobDev.Search.Sbert;

public class SBertClient
{
    private readonly IOptions<SBertOptions> _options;
    private readonly HttpClient _httpClient; //TODO: this should be done differently

    public SBertClient(IOptions<SBertOptions> options)
    {
        _options = options;
        _httpClient = new() { BaseAddress = new Uri(_options.Value.Url) };
    }

    public async Task<float[]> GetEmbeddingAsync(string input)
    {
        var results = await _httpClient.GetAsync($"/generate-embedding?query={input}");
        var json = await results.Content.ReadAsStringAsync();
        var node = JsonSerializer.Deserialize<JsonNode>(json);
        var array = (JsonArray)node["embedding"];
        var floats = array.Select(i => (float)i).ToArray();
        return floats;
    }

    public async Task<double[]> GetEmbeddingDoubleAsync(string input)
    {
        var results = await _httpClient.GetAsync($"/generate-embedding?query={input}");
        var json = await results.Content.ReadAsStringAsync();
        var node = JsonSerializer.Deserialize<JsonNode>(json);
        var array = (JsonArray)node["embedding"];
        var floats = array.Select(i => (double)i).ToArray();
        return floats;
    }

}
