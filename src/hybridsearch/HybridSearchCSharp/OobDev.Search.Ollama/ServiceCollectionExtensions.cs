using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OobDev.Search.Ollama;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddOllamaServices(this IServiceCollection services)
    {
        services.TryAddTransient<IOllamaApiClientFactory, OllamaApiClientFactory>();
        services.TryAddTransient(sp => ActivatorUtilities.CreateInstance<MessageCompletion>(sp));
        services.TryAddTransient(sp => sp.GetRequiredService<IOllamaApiClientFactory>().Build(""));
        services.TryAddTransient<IMessageCompletion, MessageCompletion>();

        return services;
    }
}
