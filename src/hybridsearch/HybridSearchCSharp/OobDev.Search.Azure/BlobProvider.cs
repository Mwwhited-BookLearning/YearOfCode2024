using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OobDev.Search.Models;
using OobDev.Search.Linq;

namespace OobDev.Search.Providers;

public class BlobProvider :
    IStoreContent,
    ISearchContent<BlobItem>,
    ISearchContent<SearchResultModel>,
    IGetContent<ContentReference>,
    IGetSummary<ContentReference>
{
    private readonly BlobContainerClient _blockBlobClient;

    public BlobProvider(BlobServiceClient client, string collectionName)
    {
        _blockBlobClient = client.GetBlobContainerClient(collectionName);
        _blockBlobClient.CreateIfNotExists();
    }

    public async Task<ContentReference?> GetContentAsync(string file)
    {
        var blob = _blockBlobClient.GetBlobClient(file);

        if (!await blob.ExistsAsync())
            return null;

        var result = await blob.DownloadStreamingAsync();
        return new ContentReference
        {
            Content = result.Value.Content,
            ContentType = result.Value.Details.ContentType,
            FileName = Path.GetFileName(file)
        };
    }

    public Task<ContentReference?> GetSummaryAsync(string file) => GetContentAsync(file);

    public IAsyncEnumerable<SearchResultModel> QueryAsync(string? queryString, int limit = 25, int page = 0) =>
        from item in ((ISearchContent<BlobItem>)this).QueryAsync(queryString, limit, page)
        select new SearchResultModel
        {
            Content = "", //TODO: do something else here
            File = item.Metadata["File"],
            PathHash = item.Metadata["PathHash"],
            Score = 1,
            Type = SearchTypes.None,
        };
    IAsyncEnumerable<BlobItem> ISearchContent<BlobItem>.QueryAsync(string? queryString, int limit = 25, int page = 0) =>
        _blockBlobClient.GetBlobsAsync(); //todo: add query but who really cares

    public async Task<bool> TryStoreAsync(string full, string file, string pathHash)
    {
        var blob = _blockBlobClient.GetBlobClient(file);
        if (!await blob.ExistsAsync())
        {
            // Check if file exists in blob store
            //  If not exist upload
            Console.WriteLine($"upload -> {file}");
            var contentInfo = await blob.UploadAsync(full, overwrite: false);

            // https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-properties-metadata
            await blob.SetMetadataAsync(new Dictionary<string, string>
            {
                ["File"] = file,
                ["OriginalFile"] = full,
                ["PathHash"] = pathHash,
            });

            return true;
        }

        return false;
    }

}
