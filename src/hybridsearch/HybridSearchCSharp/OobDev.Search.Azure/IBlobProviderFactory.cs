namespace OobDev.Search.Providers
{
    public interface IBlobProviderFactory
    {
        BlobProvider Create(string collectionName);
    }
}