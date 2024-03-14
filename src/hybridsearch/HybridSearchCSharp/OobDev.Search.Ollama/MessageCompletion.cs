using OllamaSharp;
using System;
using System.Threading.Tasks;

namespace OobDev.Search.Ollama;

public class MessageCompletion : IMessageCompletion
{
    private readonly OllamaApiClient _client;

    public MessageCompletion(
        OllamaApiClient client
        )
    {
        _client = client;
    }

    public async Task<string> GetCompletionAsync(string modelName, string prompt) =>
        (await _client.GetCompletion(new()
        {
            Model = modelName,
            Prompt = prompt,
        })).Response;

}
