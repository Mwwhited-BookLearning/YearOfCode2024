using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using OobDev.Search.Models;

namespace OobDev.Search.Providers;

public class BlobProvider : 
    IStoreContent, 
    ISearchContent<BlobItem>, 
    IGetContent<ContentReference>
{
    private readonly BlobContainerClient _blockBlobClient;

    public BlobProvider(string connectionString, string storeName)
    {
        var blobStore = new BlobServiceClient(connectionString);
        Console.WriteLine($"connect to blobs");
        _blockBlobClient = blobStore.GetBlobContainerClient(storeName);
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

    public IAsyncEnumerable<BlobItem> QueryAsync(string? queryString, int limit = 25, int page = 0) =>
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
