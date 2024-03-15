namespace OobDev.Documents;

public interface IDocumentConversionChainBuilder
{
    ChainStep[] Steps(string sourceContentType, string destinationContentType);
}
