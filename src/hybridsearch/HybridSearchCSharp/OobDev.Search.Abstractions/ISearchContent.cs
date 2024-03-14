using System.Collections.Generic;

namespace OobDev.Search;

public interface ISearchContent<T>
{
    IAsyncEnumerable<T> QueryAsync(string? queryString, int limit = 25, int page = 0);
}
