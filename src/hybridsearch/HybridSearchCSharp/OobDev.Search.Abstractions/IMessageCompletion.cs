using System.Threading.Tasks;

namespace OobDev.Search;

public interface IMessageCompletion
{
    Task<string> GetCompletionAsync(string modelName, string prompt);
}