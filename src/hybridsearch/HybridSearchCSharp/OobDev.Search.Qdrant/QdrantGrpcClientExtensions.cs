using Microsoft.Extensions.Logging;
using Qdrant.Client.Grpc;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Search.Qdrant;

public static class QdrantGrpcClientExtensions
{
    // https://github.com/qdrant/qdrant-dotnet

    public static async Task<QdrantGrpcClient> EnsureCollectionExistsAsync(
        this QdrantGrpcClient qrdant,
        string collectionName,
        ulong vectorSize = 4096,
        Distance distanceCalculation = Distance.Cosine,
        ILogger? logger = null
        )
    {
        var collectionsResponse = await qrdant.Collections.ListAsync(new() { });
        foreach (var collection in collectionsResponse.Collections)
            logger?.LogInformation($"List: {collection}", collection);
        if (!collectionsResponse.Collections.Any(d => d.Name == collectionName))
        {
            logger?.LogInformation("create {collection} on qdrant", collectionName);
            await qrdant.Collections.CreateAsync(
                new()
                {
                    CollectionName = collectionName,
                    VectorsConfig = new()
                    {
                        Params = new()
                        {
                            Size = vectorSize,
                            Distance = distanceCalculation,
                        },
                    }
                });
            logger?.LogInformation("created {collection} on qdrant", collectionName);
        }

        return qrdant;
    }
}
