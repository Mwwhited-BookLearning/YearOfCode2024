namespace OobDev.Documents;

public interface IDocumentType
{
    string Name { get; }
    string[] ContentTypes { get; }
    string[] FileExtensions { get; }
    byte[] FileHeader { get; }
}
