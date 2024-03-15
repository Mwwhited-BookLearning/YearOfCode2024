namespace OobDev.Documents;

public interface IDocumentConversionHandler : IDocumentConversion
{
    string[] Sources { get; }
    bool SupportedSource(string contentType);
    string[] Destinations { get; }
    bool SupportedDestination(string contentType);
}
