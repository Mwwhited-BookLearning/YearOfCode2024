using Qdrant.Client.Grpc;

namespace OobDev.Search.Qdrant
{
    public interface IQdrantGrpcClientFactory
    {
        QdrantGrpcClient Create();
    }
}