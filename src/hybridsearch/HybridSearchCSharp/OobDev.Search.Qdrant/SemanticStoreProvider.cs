using OobDev.Search.IO;
using OobDev.Search.Models;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Search.Qdrant;

public class SemanticStoreProvider :
    IStoreContent,
    ISearchContent<ScoredPoint>,
    ISearchContent<SearchResultModel>
{
    private readonly IEmbeddingProvider _embedding;
    private readonly QdrantGrpcClient _vectoreStore;
    private readonly string _collectionName;
    private readonly bool _forSummary;

    public SemanticStoreProvider(
        QdrantGrpcClient vectoreStore,
        IEmbeddingProvider embedding,
        string storeName,
        bool forSummary
        )
    {
        _embedding = embedding;

        _collectionName = storeName;
        _forSummary = forSummary;

        _vectoreStore = vectoreStore;
        var vectorCollections = _vectoreStore.Collections.List(new ListCollectionsRequest { });
        if (!vectorCollections.Collections.Any(c => c.Name == _collectionName))
            _vectoreStore.Collections.Create(new CreateCollection
            {
                CollectionName = _collectionName,
                VectorsConfig = new VectorsConfig
                {
                    Params = new VectorParams
                    {
                        Size = (ulong)_embedding.Length,
                        Distance = Distance.Cosine,
                    },
                }
            });

    }

    public async IAsyncEnumerable<SearchResultModel> QueryAsync(string? queryString, int limit = 25, int page = 0)
    {
        await foreach (var item in ((ISearchContent<ScoredPoint>)this).QueryAsync(queryString, 25, 0))
            yield return new()
            {
                Score = item.Score,
                PathHash = item.Payload[nameof(SearchResultModel.PathHash)].StringValue,
                Content = item.Payload[nameof(SearchResultModel.Content)].StringValue,
                File = item.Payload[nameof(SearchResultModel.File)].StringValue,
                Type = SearchTypes.Semantic,
            };
    }

    async IAsyncEnumerable<ScoredPoint> ISearchContent<ScoredPoint>.QueryAsync(string? queryString, int limit, int page)
    {
        if (string.IsNullOrWhiteSpace(queryString))
            yield break;

        var embedding = await _embedding.GetEmbeddingAsync(queryString);

        //todo: do something with paging

        // https://qdrant.tech/documentation/concepts/search/#search-groups
        var results = await _vectoreStore.Points.SearchGroupsAsync(new SearchPointGroups
        {
            CollectionName = _collectionName,
            Limit = (uint)limit,
            Vector = { embedding },
            WithPayload = true,

            GroupBy = "PathHash",
            GroupSize = 1,
        });
        foreach (var item in results.Result.Groups.SelectMany(h => h.Hits))
            yield return item;
    }

    public async Task<bool> TryStoreAsync(string full, string file, string pathHash)
    {
        if ((await _vectoreStore.Points.ScrollAsync(new()
        {
            CollectionName = _collectionName,
            Limit = 1,
            Filter = new Filter
            {
                Must =
                    {
                        new Condition
                        {
                            Field = new FieldCondition
                            {
                                Key= "PathHash",
                                Match = new Match
                                {
                                    Text = pathHash,
                                }
                            }
                        },
                        new Condition
                        {
                            Field = new FieldCondition
                            {
                                Key= "ContentType",
                                Match = new Match
                                {
                                    Text = _forSummary? "Summary": "Content",
                                }
                            }
                        }
                    }
            }
        })).Result.Any())
        {
            return false;
        }

        // check if file indexed in vector store
        //  if not chunk file and index with embeddings 

        Console.WriteLine($"embedding and index -> {file}");

        var embeddings = new Collection<PointStruct>();

        // generate embedding for file name
        if (!_forSummary)
        {
            var value = await _embedding.GetEmbeddingAsync(file);
            embeddings.Add(new PointStruct
            {
                Id = new PointId { Uuid = Guid.NewGuid().ToString() },
                Payload = {
                            ["File"]= file,
                            ["OriginalFile"]= full,
                            ["PathHash"] = pathHash,

                            ["FileName"]= Path.GetFileNameWithoutExtension(file),
                            ["Directory"]= Path.GetDirectoryName(file)??"",
                            ["Extensions"]= Path.GetExtension(file),

                            ["Content"] = file,
                            ["ContentType"] = "FileName",

                            ["Sequence"] = 0,
                            ["Start"] = 0,
                            ["Length"] = file.Length,

                        },
                Vectors = new Vectors()
                {
                    Vector = value
                },
            });
        }

        //break content and generate embeddings
        await foreach (var chunk in FileTools.SplitFileAsync(full, _embedding.Length, 0))
        {
            var value = await _embedding.GetEmbeddingAsync(chunk.Data);
            embeddings.Add(new PointStruct
            {
                Id = new PointId { Uuid = Guid.NewGuid().ToString() },
                Payload = {
                            ["File"]= file,
                            ["OriginalFile"]= full,
                            ["PathHash"] = pathHash,

                            ["FileName"]= Path.GetFileNameWithoutExtension(file),
                            ["Directory"]= Path.GetDirectoryName(file)??"",
                            ["Extensions"]= Path.GetExtension(file),

                            ["Content"] = chunk.Data,
                            ["ContentType"] =_forSummary? "Summary": "Content",

                            [nameof(chunk.Sequence)] = chunk.Sequence,
                            [nameof(chunk.Start)] = chunk.Start,
                            [nameof(chunk.Length)] = chunk.Length,
                        },
                Vectors = new Vectors()
                {
                    Vector = value
                },
            });
        }

        await _vectoreStore.Points.UpsertAsync(new()
        {
            CollectionName = _collectionName,
            Points = { embeddings },
        });

        return true;
    }

}