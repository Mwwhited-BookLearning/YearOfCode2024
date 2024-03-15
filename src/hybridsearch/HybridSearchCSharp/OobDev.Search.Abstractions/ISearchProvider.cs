using OobDev.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OobDev.Search
{
    public interface ISearchProvider
    {
        Task<ContentReference?> DownloadAsync(string file);
        Task<float[]> EmbedAsync(string text);
        Task<IEnumerable<SearchResultWithSummaryModel>> HybridSearchAsync(string? query, int limit);
        Task<IEnumerable<SearchResultWithSummaryModel>> LexicalSearchAsync(string? query, int limit);
        Task<IEnumerable<SearchResultModel>> ListAsync();
        Task<IEnumerable<SearchResultWithSummaryModel>> SemanticSearchAsync(string? query, int limit);
        Task<ContentReference?> SummaryAsync(string file);
    }
}