using OobDev.Search.Qdrant;

namespace OobDev.Search.Providers
{
    public interface ISemanticStoreProviderFactory
    {
        SemanticStoreProvider Create(bool forSummary);
    }
}