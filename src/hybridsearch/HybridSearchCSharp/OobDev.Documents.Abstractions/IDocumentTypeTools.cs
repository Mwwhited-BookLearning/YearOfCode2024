using System.IO;

namespace OobDev.Documents;

public interface IDocumentTypeTools
{
    IDocumentType? GetByContentType(string contentType);
    IDocumentType? GetByFileExtension(string fileExtension);
    IDocumentType? GetByFileHeader(Stream stream);
}