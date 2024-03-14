using System.Threading.Tasks;

namespace OobDev.Search;

public interface ISummerizeContent
{
    Task<string> GenerateSummaryAsync(string input);
}

