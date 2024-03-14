using System.Text;
using System.Threading.Tasks;

namespace OobDev.Search.Providers;

public class DocumentSummaryGenerationProvider : ISummerizeContent
{
    private const int MAX_LENGTH = 4096; //TODO: should look this up from model.

    private readonly IMessageCompletion _messageCompletion;
    private readonly string _modelName;
    private readonly string _promptTemplate;

    public DocumentSummaryGenerationProvider(
        IMessageCompletion messageCompletion,
        string modelName = "mistral:instruct",
        string promptTemplate = @"Write a single paragraph of less than 5 sentences to summarize the following text 
{0}"
        )
    {
        _messageCompletion = messageCompletion;
        _modelName = modelName;
        _promptTemplate = promptTemplate;
        ;
    }

    public async Task<string> GenerateSummaryAsync(string content)
    {
        // if (content.Length < MAX_LENGTH) return content;

        var x = 0;
        var chunks = content.ReplaceLineEndings(" ").SplitBy(MAX_LENGTH);
        var sb = new StringBuilder();
        foreach (var chunk in chunks)
        {
            x++;
            var result = await GetCompletionAsync(_modelName, _promptTemplate, chunk);
            sb.Append(result).Append(' ');
        }
        if (x <= 1)
            return sb.ToString();
        var final = await GetCompletionAsync(_modelName, _promptTemplate, sb.ToString());
        //TODO: consider doing something better here for really long text
        return final;
    }

    internal async Task<string> GetCompletionAsync(string modelName, string promptTemplate, string content)
    {
        var prompt = string.Format(promptTemplate, content);

        var completion = await _messageCompletion.GetCompletionAsync(modelName,prompt);

        return completion;
    }
}
