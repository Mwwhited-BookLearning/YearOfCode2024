using System.Threading.Tasks;

namespace OobDev.Search;

public interface IGetSummary<T>
{
    Task<T?> GetSummaryAsync(string file);
}