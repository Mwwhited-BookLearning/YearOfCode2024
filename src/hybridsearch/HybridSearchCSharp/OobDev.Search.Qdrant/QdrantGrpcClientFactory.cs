using Qdrant.Client.Grpc;

namespace OobDev.Search.Qdrant;

public class QdrantGrpcClientFactory
{
    public QdrantGrpcClient Build(string hostName, int port = 6334) =>
         new QdrantGrpcClient(QdrantChannel.ForAddress($"http://{hostName}:{port}"));
}
