using System.Collections.Generic;
using System.Threading.Tasks;

namespace OobDev.Search.Linq;

public static class EnumerableExtensions
{
    public static async Task<IEnumerable<T>> ToSetAsync<T>(this IAsyncEnumerable<T> items)
    {
        var result = new List<T>();
        await foreach (var item in items)
            result.Add(item);
        return result;
    }
}
