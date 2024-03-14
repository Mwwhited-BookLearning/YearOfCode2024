using System.Threading.Tasks;

namespace OobDev.Search;

public interface IGetContent<T>
{
    Task<T?> GetContentAsync(string file);
}
