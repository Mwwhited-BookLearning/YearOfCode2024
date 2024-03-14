using System.Text.Json.Nodes;
using System.Text.Json;

namespace OobDev.Search.Sbert;

public class SBertClient(
        string baseUrl
    )
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri(baseUrl) };

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
