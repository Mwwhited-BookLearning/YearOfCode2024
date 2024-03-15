using OpenSearch.Net;

namespace OobDev.Search.OpenSearch
{
    public interface IOpenSearchClientFactory
    {
        IOpenSearchLowLevelClient Create();
    }
}