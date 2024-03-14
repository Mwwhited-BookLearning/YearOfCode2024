using OobDev.Search.Models;
using Qdrant.Client.Grpc;
using System;
using System.IO;

namespace OobDev.Search.Qdrant;

public class PointStructFactory : IPointStructFactory
{
    public const string ServiceInstanceType = "service-instance";

    public PointStruct CreateFileChunk(FileMetaData metadata, ContentChunk chunk, FileInfo fileInfo, float[] vector) =>
        new()
        {
            Id = new() { Uuid = metadata.Uuid },
            Payload = {
                [nameof(metadata.Path)] = metadata.Path,
                [nameof(metadata.Path) + nameof(metadata.Hash)] = Convert.ToBase64String(metadata.Hash),
                [nameof(metadata.Path) + nameof(metadata.Hash) + "Id"] =new Guid(metadata.Hash).ToString(),
                [nameof(metadata.BasePath)] = metadata.BasePath,

                [$"chunk_{nameof(chunk.Data)}"] =chunk.Data,
                [$"chunk_{nameof(chunk.Length)}"] =chunk.Length,
                [$"chunk_{nameof(chunk.Sequence)}"] =chunk.Sequence,
                [$"chunk_{nameof(chunk.Start)}"] =chunk.Start,

                [$"file_{nameof(fileInfo.Length)}"] =fileInfo.Length,
                [$"file_{nameof(fileInfo.LastAccessTime)}"] =fileInfo.LastAccessTime.ToString("yyyyMMddHHmmss"),
                [$"file_{nameof(fileInfo.LastWriteTime)}"] =fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss"),
                [$"file_{nameof(fileInfo.CreationTime)}"] =fileInfo.CreationTime.ToString("yyyyMMddHHmmss"),
                [$"file_{nameof(fileInfo.Extension)}"] =fileInfo.Extension,
            },

            Vectors = new Vectors { Vector = vector, }
        };

    public PointStruct CreateServiceReference(
        Guid uuid,
        Type serviceType,
        string description,
        float[] vector
        ) =>
        new()
        {
            Id = new() { Uuid = uuid.ToString() },

            Payload =
            {
                [nameof( serviceType.Name)] = serviceType.Name,
                [nameof( serviceType.FullName)] = serviceType.FullName ?? serviceType.Name,
                [nameof( serviceType.AssemblyQualifiedName)] = serviceType.AssemblyQualifiedName ?? serviceType.Name,
                [nameof( serviceType.Namespace)] = serviceType.Namespace ?? serviceType.Name,
                [nameof( description)] = description,

                ["type"]= ServiceInstanceType,
            },

            Vectors = new Vectors { Vector = vector, }
        };

    public PointStruct CreateQuestion(
        Guid uuid,
        string question,
        float[] vector,
        string? type = null
        ) =>
        new()
        {
            Id = new() { Uuid = uuid.ToString() },

            Payload =
            {
                [nameof( question)] = question,

                ["type"]= type ?? nameof(question),
            },

            Vectors = new Vectors { Vector = vector, }
        };
}
