using System;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

public record Document
{
    public Document()
    {
        DocumentNumber = string.Empty;
    }

    public Document(DocumentType? type, string number)
    {
        if (type == null)
        {
            throw new ArgumentException("core.error.documentType.notNull");
        }
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("core.error.documentNumber.notBlank");
        }
        DocumentType = type.Value;
        DocumentNumber = number;
    }

    public Document(string type, string number)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("core.error.documentType.notNull");
        }
        if (!Enum.TryParse<DocumentType>(type, true, out var parsedType))
        {
            throw new ArgumentException("core.error.documentType.invalid");
        }
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("core.error.documentNumber.notBlank");
        }
        DocumentType = parsedType;
        DocumentNumber = number;
    }

    public DocumentType DocumentType { get; init; }
    public string DocumentNumber { get; init; }
}
