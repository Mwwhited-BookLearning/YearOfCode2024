using System.Threading.Tasks;

namespace OobDev.Search;

public interface IStoreContent
{
    Task<bool> TryStoreAsync(string full, string file, string pathHash);
}
