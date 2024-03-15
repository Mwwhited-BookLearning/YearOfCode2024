using Microsoft.AspNetCore.Mvc;
using OobDev.Search.Models;

namespace OobDev.Search.WebApi.Controllers;

[Route("[Controller]/[Action]")]
public class FileController : Controller
{
    private readonly ISearchProvider _search;

    public FileController(
        ISearchProvider search
        )
    {
        _search = search;
    }


    //public Task<ContentReference?> DownloadAsync(string file)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<ContentReference?> SummaryAsync(string file)
    //{
    //    throw new NotImplementedException();
    //}

    [HttpGet("{*file}")]
    public async Task<IActionResult> Download(string file) =>
        await _search.DownloadAsync(file) switch
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
        await _search.SummaryAsync(file) switch
        {
            null => NotFound(),
            ContentReference blob => File(blob.Content, blob.ContentType, blob.FileName)
        };

    [HttpGet]
    public async Task<IEnumerable<SearchResultModel>> List() => await _search.ListAsync();

    [HttpGet]
    public async Task<float[]> Embed(string text) => await _search.EmbedAsync(text);

    [HttpGet]
    public async Task<IEnumerable<SearchResultWithSummaryModel>> SemanticSearch(string? query = default, int limit = 10)
    {
        Response.Headers[$"X-APP-{nameof(query)}"] = query;
        Response.Headers[$"X-APP-{nameof(limit)}"] = limit.ToString();
        return await _search.SemanticSearchAsync(query, limit);
    }

    [HttpGet]
    public async Task<IEnumerable<SearchResultWithSummaryModel>> LexicalSearch(string? query = default, int limit = 10)
    {
        Response.Headers[$"X-APP-{nameof(query)}"] = query;
        Response.Headers[$"X-APP-{nameof(limit)}"] = limit.ToString();
        return await _search.LexicalSearchAsync(query, limit);
    }

    [HttpGet]
    public async Task<IEnumerable<SearchResultWithSummaryModel>> HybridSearch(string? query = default, int limit = 10)
    {
        Response.Headers[$"X-APP-{nameof(query)}"] = query;
        Response.Headers[$"X-APP-{nameof(limit)}"] = limit.ToString();
        return await _search.HybridSearchAsync(query, limit);
    }

}
