using Microsoft.Extensions.DependencyInjection;
using OobDev.Search.Linq;
using OobDev.Search.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OobDev.Search.Providers;

public class SearchProvider : ISearchProvider
{
    private readonly ISearchContent<SearchResultModel> _semantic;
    private readonly ISearchContent<SearchResultModel> _lexical;
    private readonly ISearchContent<SearchResultModel> _hybrid;
    private readonly IEmbeddingProvider _embedding;
    private readonly ISearchContent<SearchResultModel> _contentStore;
    private readonly IGetContent<ContentReference> _content;
    private readonly IGetSummary<ContentReference> _summary;

    public SearchProvider(
        [FromKeyedServices(SearchTypes.Semantic)] ISearchContent<SearchResultModel> semantic,
        [FromKeyedServices(SearchTypes.Lexical)] ISearchContent<SearchResultModel> lexical,
        [FromKeyedServices(SearchTypes.Hybrid)] ISearchContent<SearchResultModel> hybrid,
        IEmbeddingProvider embedding,
        [FromKeyedServices(SearchTypes.None)] ISearchContent<SearchResultModel> contentStore,
        IGetContent<ContentReference> content,
        IGetSummary<ContentReference> summary
        )
    {
        _semantic = semantic;
        _lexical = lexical;
        _hybrid = hybrid;
        _embedding = embedding;
        _contentStore = contentStore;
        _content = content;
        _summary = summary;
    }

    public async Task<ContentReference?> DownloadAsync(string file) =>
        await _content.GetContentAsync(HttpUtility.UrlDecode(file));

    //[Route("md2html/{*file}")]
    //public async Task<IActionResult> Md2Html(string file)
    //{
    //    var result = await _blob.GetContentAsync(HttpUtility.UrlDecode(file));
    //    if (result == null)
    //        return NotFound();

    //    using var reader = new StreamReader(result.Content);
    //    var markdig = Markdown.Parse(reader.ReadToEnd());
    //    var html = markdig.ToHtml();

    //    var ms = new MemoryStream();
    //    var writer = new StreamWriter(ms) { AutoFlush = true, };
    //    await writer.WriteAsync(html);
    //    ms.Position = 0;
    //    return File(ms, "text/html");
    //}

    public async Task<ContentReference?> SummaryAsync(string file) =>
        await _summary.GetSummaryAsync(HttpUtility.UrlDecode(file));

    public async Task<IEnumerable<SearchResultModel>> ListAsync() =>
        await _contentStore.QueryAsync("").ToReadOnlyCollectionAsync();

    public async Task<float[]> EmbedAsync(string text) => await _embedding.GetEmbeddingAsync(text);

    public async Task<IEnumerable<SearchResultWithSummaryModel>> SemanticSearchAsync(string? query, int limit) =>
        Upgrade(await _semantic.QueryAsync(query, limit).ToReadOnlyCollectionAsync());

    public async Task<IEnumerable<SearchResultWithSummaryModel>> LexicalSearchAsync(string? query, int limit) =>
        Upgrade(await _lexical.QueryAsync(query, limit).ToReadOnlyCollectionAsync());

    public async Task<IEnumerable<SearchResultWithSummaryModel>> HybridSearchAsync(string? query, int limit) =>
        Upgrade(await _hybrid.QueryAsync(query, limit).ToReadOnlyCollectionAsync());

    private IEnumerable<SearchResultWithSummaryModel> Upgrade(IEnumerable<SearchResultModel> items) =>
        from item in items
        select new SearchResultWithSummaryModel
        {
            Content = item.Content,
            File = item.File,
            PathHash = item.PathHash,
            Score = item.Score,
            Type = item.Type,
            Summary = GetSummary(item.File) ?? "",
        };

    private string? GetSummary(string file)
    {
        var result = _summary.GetSummaryAsync(file).Result;
        if (result == null)
            return null;
        using var stream = result.Content;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
