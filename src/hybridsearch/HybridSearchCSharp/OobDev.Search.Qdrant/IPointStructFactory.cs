using OobDev.Search.Models;
using Qdrant.Client.Grpc;
using System;
using System.IO;

namespace OobDev.Search.Qdrant
{
    public interface IPointStructFactory
    {
        PointStruct CreateFileChunk(FileMetaData metadata, ContentChunk chunk, FileInfo fileInfo, float[] vector);
        PointStruct CreateQuestion(Guid uuid, string question, float[] vector, string? type = null);
        PointStruct CreateServiceReference(Guid uuid, Type serviceType, string description, float[] vector);
    }
}