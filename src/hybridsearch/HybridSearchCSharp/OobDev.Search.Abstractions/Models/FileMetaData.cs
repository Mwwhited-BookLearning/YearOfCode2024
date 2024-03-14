namespace OobDev.Search.Models;

public record FileMetaData
{
    public required string Uuid { get; init; }
    public required string Path { get; init; }
    public required byte[] Hash { get; init; }
    public required string BasePath { get; init; }
}
