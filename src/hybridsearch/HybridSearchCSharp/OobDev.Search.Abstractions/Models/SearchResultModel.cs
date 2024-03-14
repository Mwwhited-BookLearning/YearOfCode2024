namespace OobDev.Search.Models;

public record SearchResultModel
{
    public float Score { get; init; }
    public string PathHash { get; init; }
    public string File { get; init; }
    public string Content { get; init; }
    public SearchTypes Type { get; init; }
}