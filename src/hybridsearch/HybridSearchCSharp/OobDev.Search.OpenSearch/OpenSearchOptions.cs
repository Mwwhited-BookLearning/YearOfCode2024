namespace OobDev.Search.OpenSearch;

public class OpenSearchOptions
{
    public required string HostName { get; set; } = "localhost";
    public required int Port { get; set; } = 9200;
    public required string IndexName { get; set; } = "default";

    public string? UserName { get; set; }
    public string? Password { get; set; }
}
