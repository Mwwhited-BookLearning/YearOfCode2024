using System.IO;
using System.Threading.Tasks;

namespace OobDev.Documents;

public interface IDocumentConversion
{
    Task ConvertAsync(Stream source, string sourceContentType, Stream destination, string destinationContentType);
}
