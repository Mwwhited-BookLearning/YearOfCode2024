namespace OobDev.Documents;

public record ChainStep
{
    public required IDocumentConversionHandler Handler { get; init; }
    public required string SourceContentType { get; init; }
    public required string DestinationContentType { get; init; }
}

