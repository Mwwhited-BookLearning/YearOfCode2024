using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OobDev.Documents;

public class DocumentTypeTools : IDocumentTypeTools
{
    private readonly IEnumerable<IDocumentType> _types;

    public DocumentTypeTools(
        IEnumerable<IDocumentType> types
        )
    {
        _types = types;
    }

    public IDocumentType? GetByContentType(string contentType) =>
        _types.FirstOrDefault(t => t.ContentTypes.Any(i => string.Equals(i, contentType, StringComparison.OrdinalIgnoreCase)));

    public IDocumentType? GetByFileExtension(string fileExtension) =>
        _types.FirstOrDefault(t => t.FileExtensions.Any(i => string.Equals(i, fileExtension, StringComparison.OrdinalIgnoreCase)));

    public IDocumentType? GetByFileHeader(Stream stream)
    {
        var maxRead = _types.Max(t => t.FileHeader.Length);
        if (maxRead > 0)
        {
            var p = stream.Position;

            Span<byte> temp = new byte[maxRead];
            var possible = _types.Where(t => t.FileHeader.Length > 0);
            stream.Read(temp);

            foreach (var t in possible.Where(t => t.FileHeader.Length > 0))
                if (temp.StartsWith(t.FileHeader))
                    return t;

            stream.Position = 0;
        }

        return default;
    }
}
