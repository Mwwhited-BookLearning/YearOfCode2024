using Azure.Storage.Blobs;

namespace OobDev.Search.Providers
{
    public interface IBlobServiceClientFactory
    {
        BlobServiceClient Create();
    }
}