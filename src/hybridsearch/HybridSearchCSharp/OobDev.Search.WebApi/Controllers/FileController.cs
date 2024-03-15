using Microsoft.AspNetCore.Mvc;
using OobDev.Documents;
using OobDev.Search.Models;
using System.Net;

namespace OobDev.Search.WebApi.Controllers;

[Route("[Controller]/[Action]")]
public class FileController : Controller
{
    private readonly ISearchProvider _search;
    private readonly IDocumentConversion _document;

    public FileController(
        ISearchProvider search,
        IDocumentConversion document
        )
    {
        _search = search;
        _document = document;
    }

    [HttpGet("{*file}")]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Download(string file) =>
        await _search.DownloadAsync(file) switch
        {
            null => NotFound(),
            ContentReference blob => File(blob.Content, blob.ContentType, blob.FileName)
        };

    [HttpGet("{*file}")]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Text(string file) =>
        await _search.DownloadAsync(file) switch
        {
            null => NotFound(),
            ContentReference blob => File(
                await ConvertToAsync(blob.Content, blob.ContentType, "text/plain"),
                "text/plain",
                Path.ChangeExtension(blob.FileName, ".txt"))
        };

    [HttpGet("{*file}")]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Html(string file) =>
        await _search.DownloadAsync(file) switch
        {
            null => NotFound(),
            ContentReference blob => File(
                await ConvertToAsync(blob.Content, blob.ContentType, "text/html"),
                "text/html",
                Path.ChangeExtension(blob.FileName, ".html"))
        };

    [HttpGet("{*file}")]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Pdf(string file) =>
        await _search.DownloadAsync(file) switch
        {
            null => NotFound(),
            ContentReference blob => File(
                await ConvertToAsync(blob.Content, blob.ContentType, "application/pdf"),
                "application/pdf",
                Path.ChangeExtension(blob.FileName, ".pdf"))
        };

    private async Task<Stream> ConvertToAsync(Stream source, string sourceType, string destinationType)
    {
        var ms = new MemoryStream();
        await _document.ConvertAsync(source, sourceType, ms, destinationType);
        ms.Position = 0;
        return ms;
    }

    [HttpGet("{*file}")]
    [ProducesResponseType(typeof(FileStreamResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
