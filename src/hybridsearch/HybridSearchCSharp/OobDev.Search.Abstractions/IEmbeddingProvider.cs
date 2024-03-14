using System.Threading.Tasks;

namespace OobDev.Search;

public interface IEmbeddingProvider
{
    int Length { get; }
    Task<float[]> GetEmbeddingAsync(string content);
}
