using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace OobDev.Search.Linq;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<TResult> Select<T, TResult>(
        this IAsyncEnumerable<T> items,
        Func<T, TResult> map
        )
    {
        await foreach (var item in items)
            yield return map(item);
    }

    public static async IAsyncEnumerable<TResult> Select<T, TResult>(
        this IAsyncEnumerable<T> items,
        Func<T, Task<TResult>> map
        )
    {
        await foreach (var item in items)
            yield return await map(item);
    }


    public static IEnumerable<T> AsEnumerable<T>(this IAsyncEnumerable<T> items) =>
        items.ToBlockingEnumerable();


    public static async Task<IReadOnlyCollection<T>> ToReadOnlyCollectionAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        var collection = new Collection<T>();
        await foreach (var item in items)
            collection.Add(item);
        return collection;
    }
}
