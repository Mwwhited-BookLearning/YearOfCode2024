using Microsoft.Extensions.Logging;
using OllamaSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Search.Ollama;

public static class OllamaApiClientExtensions
{
    public static async Task<OllamaApiClient> EnsureModelExistsAsync(
        this OllamaApiClient ollama,
        string modelName,
        ILogger logger 
        )
    {
        logger.LogInformation("list models");
        var models = await ollama.ListLocalModels();
        foreach (var model in models)
            Console.WriteLine($"model: {model.Name}, size: {model.Size} ");

        logger.LogInformation("create maybe");
        if (models.Any(m => !m.Name.StartsWith(modelName)))
            await ollama.CopyModel("llama2", modelName);
        logger.LogInformation("Select model: {modelName}", modelName);

        //ollama.SelectedModel = modelName;

        return ollama;
    }

    public static float[] GetEmbeddingSingle(
    this OllamaApiClient client,
    string text,
    string modelName) =>
            client.GetEmbeddingSingleAsync(text, modelName).Result;

    public static async Task<float[]> GetEmbeddingSingleAsync(
        this OllamaApiClient client,
        string text,
        string modelName
        )
    {
        var doubles = await client.GetEmbeddingDoubleAsync(text, modelName);
        var floats = new float[doubles.Length * 2];
        for (var i = 0; i < doubles.Length; i++)
        {
            floats[i * 2] = (float)doubles[i];
            floats[i * 2 + 1] = (float)(doubles[i] * 10000000 - (float)doubles[i] * 10000000);
        }
        return floats;
    }

    public static async Task<double[]> GetEmbeddingDoubleAsync(
        this OllamaApiClient client,
        string text,
        string modelName) =>
            (await client.GenerateEmbeddings(new() { Model = modelName, Prompt = text })).Embedding;

    public static double[] GetEmbeddingDouble(
        this OllamaApiClient client,
        string text,
        string modelName) =>
            client.GetEmbeddingDoubleAsync(text, modelName).Result;
}