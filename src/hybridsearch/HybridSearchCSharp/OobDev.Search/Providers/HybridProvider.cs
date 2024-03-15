using Microsoft.Extensions.DependencyInjection;
using OobDev.Search.Linq;
using OobDev.Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OobDev.Search.Providers;

public class HybridProvider : ISearchContent<SearchResultModel>
{
    private readonly ISearchContent<SearchResultModel> _lexicalProvider;
    private readonly ISearchContent<SearchResultModel> _semanticStoreProvider;

    public HybridProvider(
        [FromKeyedServices(SearchTypes.Lexical)] ISearchContent<SearchResultModel> lexicalProvider,
        [FromKeyedServices(SearchTypes.Semantic)] ISearchContent<SearchResultModel> semanticStoreProvider
        )
    {
        _lexicalProvider = lexicalProvider;
        _semanticStoreProvider = semanticStoreProvider;
    }

    public async IAsyncEnumerable<SearchResultModel> QueryAsync(string? queryString, int limit = 25, int page = 0)
    {
        var lexical = _lexicalProvider.QueryAsync(queryString, limit * 2, page).ToReadOnlyCollectionAsync();
        var semantic = _semanticStoreProvider.QueryAsync(queryString, limit * 2, page).ToReadOnlyCollectionAsync();

        await Task.WhenAll(lexical, semantic);

        var left = from l in lexical.Result
                   join s in semantic.Result on l.PathHash equals s.PathHash into temp
                   from s in temp.DefaultIfEmpty()
                   select new
                   {
                       hash = l.PathHash,
                       l,
                       s,
                   };

        var right = from s in semantic.Result
                    join l in lexical.Result on s.PathHash equals l.PathHash into temp
                    from l in temp.DefaultIfEmpty()
                    select new
                    {
                        hash = s.PathHash,
                        l,
                        s,
                    };

        var unioned = left.UnionBy(right, k => k.hash);
        var maxLex = unioned.Max(i => i.l?.Score);

        var reranked = from i in unioned
                       let lScore = i.l?.Score
                       let sScore = i.s?.Score

                       let rLScore = (lScore.HasValue && maxLex > 1) ? lScore / maxLex : 0f
                       let rSScore = sScore ?? 0f
                       let r = rLScore > rSScore ? rLScore : rSScore
                       orderby r descending
                       select new
                       {
                           r,
                           i.hash,
                           i.l,
                           i.s,
                       };

        var mapped = from u in reranked
                     select new SearchResultModel
                     {
                         Score = u.r ?? 0,

                         PathHash = u.hash,

                         File = u.l?.File ?? u.s?.File ?? "",
                         Content = u.l?.Content ?? u.s?.Content ?? "",

                         Type = (u.l?.Type ?? SearchTypes.None) | (u.s?.Type ?? SearchTypes.None),
                     };

        var results = mapped.Take(limit).ToArray();

        foreach (var i in results)
            yield return i;
    }
}
