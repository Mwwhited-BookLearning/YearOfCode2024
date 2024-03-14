namespace OobDev.Search.Sbert;

public class SentenceEmbeddingProvider : IEmbeddingProvider
{
    private readonly SBertClient _client;

    public SentenceEmbeddingProvider(
        SBertClient client
    )
    {
        _client = client;
        Console.WriteLine($"connect to embeddings");
    }

    private int? _length;
    public int Length => _length ??= GetEmbeddingAsync("hello world").Result.Length;

    public Task<float[]> GetEmbeddingAsync(string content) => _client.GetEmbeddingAsync(content);
}