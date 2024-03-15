namespace OobDev.Search.Providers;

public class AzureBlobProviderOptions
{
    public required string ConnectionString { get; set; }
    public required string DocumentCollectionName { get; set; }
    public required string SummaryCollectionName { get; set; }
}
