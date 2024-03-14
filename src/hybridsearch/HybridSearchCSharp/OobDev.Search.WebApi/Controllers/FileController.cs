using Microsoft.AspNetCore.Mvc;
using OobDev.Search.Linq;
using OobDev.Search.Models;
using OobDev.Search.Providers;
using System.Web;

namespace OobDev.Search.WebApi.Controllers;

public class FileController : Controller
{
    private const string storeName = "docs";
    private const string hostName = "192.168.1.170";

    private readonly BlobProvider _blob;
    private readonly BlobProvider _sblob;

    private readonly ISearchContent<SearchResultModel> _semantic;
    private readonly ISearchContent<SearchResultModel> _lexical;
    private readonly ISearchContent<SearchResultModel> _hybrid;
    private readonly IEmbeddingProvider _embedding;

    public FileController(
        [FromKeyedServices(SearchTypes.Semantic)] ISearchContent<SearchResultModel> semantic,
        [FromKeyedServices(SearchTypes.Lexical)] ISearchContent<SearchResultModel> lexical,
        [FromKeyedServices(SearchTypes.Hybrid)] ISearchContent<SearchResultModel> hybrid,
        IEmbeddingProvider embedding
        )
    {
        _blob = new BlobProvider(
            connectionString: "DefaultEndpointsProtocol=https;" +
                "AccountName=devstoreaccount1;" +
                "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
                $"BlobEndpoint=http://{hostName}:10000/devstoreaccount1;",
            storeName: storeName
                );
        _sblob = new BlobProvider(
            connectionString: "DefaultEndpointsProtocol=https;" +
                "AccountName=devstoreaccount1;" +
                "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
                $"BlobEndpoint=http://{hostName}:10000/devstoreaccount1;",
            storeName: "summary"
                );

        _semantic = semantic;
        _lexical = lexical;
        _hybrid = hybrid;
        _embedding= embedding;
    }

    [Route("download/{*file}")]
    public async Task<IActionResult> Download(string file) =>
        await _blob.GetContentAsync(HttpUtility.UrlDecode(file)) switch
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

    [Route("summary/{*file}")]
    public async Task<IActionResult> Summary(string file) =>
        await _sblob.GetContentAsync(HttpUtility.UrlDecode(file)) switch
        {
            null => NotFound(),
            ContentReference blob => File(blob.Content, blob.ContentType, blob.FileName)
        };

    public async Task<IActionResult> Index()
    {
        var blobs = await _blob.QueryAsync("").ToReadOnlyCollectionAsync();
        return View(blobs);
    }

    public async Task<IActionResult> Embed(string text) => Json(await _embedding.GetEmbeddingAsync(text));

    public async Task<IActionResult> SemanticSearch(string? query = default, int limit = 10)
    {
        ViewData[nameof(query)] = query;
        ViewData[nameof(limit)] = limit;
        var results = await this._semantic.QueryAsync(query, limit).ToReadOnlyCollectionAsync();
        return View("SearchResults", Upgrade(results));
    }

    public async Task<IActionResult> LexicalSearch(string? query = default, int limit = 10)
    {
        ViewData[nameof(query)] = query;
        ViewData[nameof(limit)] = limit;
        var results = await this._lexical.QueryAsync(query, limit).ToReadOnlyCollectionAsync();
        return View("SearchResults", Upgrade(results));
    }

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
            Summary = GetContent(item.File) ?? "",
        };

    private string? GetContent(string file)
    {
        var result = _sblob.GetContentAsync(file).Result;
        if (result == null)
            return null;
        using var stream = result.Content;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
