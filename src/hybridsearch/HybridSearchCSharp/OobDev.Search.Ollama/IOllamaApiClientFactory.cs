using OllamaSharp;

namespace OobDev.Search.Ollama;

public interface IOllamaApiClientFactory
{
    OllamaApiClient Build(string host);
}
