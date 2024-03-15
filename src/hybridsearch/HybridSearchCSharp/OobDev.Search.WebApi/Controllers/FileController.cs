using Microsoft.AspNetCore.Mvc;
using OobDev.Search.Linq;
using OobDev.Search.Models;
using System.Web;

namespace OobDev.Search.WebApi.Controllers;

[Route("[Controller]/[Action]")]
public class FileController : Controller
{
    private readonly ISearchContent<SearchResultModel> _semantic;
    private readonly ISearchContent<SearchResultModel> _lexical;
    private readonly ISearchContent<SearchResultModel> _hybrid;
    private readonly IEmbeddingProvider _embedding;
    private readonly ISearchContent<SearchResultModel> _contentStore;
    private readonly IGetContent<ContentReference> _content;
    private readonly IGetSummary<ContentReference> _summary;

    public FileController(
        [FromKeyedServices(SearchTypes.Semantic)] ISearchContent<SearchResultModel> semantic,
        [FromKeyedServices(SearchTypes.Lexical)] ISearchContent<SearchResultModel> lexical,
        [FromKeyedServices(SearchTypes.Hybrid)] ISearchContent<SearchResultModel> hybrid,
        IEmbeddingProvider embedding,
        ISearchContent<SearchResultModel> contentStore,
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

    [HttpGet("{*file}")]
    public async Task<IActionResult> Download(string file) =>
        await _content.GetContentAsync(HttpUtility.UrlDecode(file)) switch
        {
            null => NotFound(),
            ContentReference blob => File(blob.Content, blob.ContentType, blob.FileName)
        };

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

    [HttpGet("{*file}")]
    public async Task<IActionResult> Summary(string file) =>
        await _summary.GetSummaryAsync(HttpUtility.UrlDecode(file)) switch
        {
            null => NotFound(),
            ContentReference blob => File(blob.Content, blob.ContentType, blob.FileName)
        };

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var blobs = await _contentStore.QueryAsync("").ToReadOnlyCollectionAsync();
        return View(blobs);
    }

    [HttpGet]
    public async Task<IActionResult> Embed(string text) => Json(await _embedding.GetEmbeddingAsync(text));

    [HttpGet]
    public async Task<IActionResult> SemanticSearch(string? query = default, int limit = 10)
    {
        ViewData[nameof(query)] = query;
        ViewData[nameof(limit)] = limit;
        var results = await this._semantic.QueryAsync(query, limit).ToReadOnlyCollectionAsync();
        return View("SearchResults", Upgrade(results));
    }

    [HttpGet]
    public async Task<IActionResult> LexicalSearch(string? query = default, int limit = 10)
    {
        ViewData[nameof(query)] = query;
        ViewData[nameof(limit)] = limit;
        var results = await this._lexical.QueryAsync(query, limit).ToReadOnlyCollectionAsync();
        return View("SearchResults", Upgrade(results));
    }

    [HttpGet]
    public async Task<IActionResult> HybridSearch(string? query = default, int limit = 10)
    {
        ViewData[nameof(query)] = query;
        ViewData[nameof(limit)] = limit;
        var results = await this._hybrid.QueryAsync(query, limit).ToReadOnlyCollectionAsync();
        return View("SearchResults", Upgrade(results));
    }

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
