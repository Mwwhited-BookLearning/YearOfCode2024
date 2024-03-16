namespace OobDev.Documents.Models;

public record DocumentType : IDocumentType
{
    public required string Name { get; init; }
    public required string[] ContentTypes { get; init; }
    public required string[] FileExtensions { get; init; }
    public required byte[] FileHeader { get; init; }
}
