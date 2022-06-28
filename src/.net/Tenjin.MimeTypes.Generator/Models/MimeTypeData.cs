namespace Tenjin.MimeTypes.Generator.Models;

public record MimeTypeData
{
    public IEnumerable<MimeTypeRecord> FileExtensionToMimeTypes { get; init; } = Enumerable.Empty<MimeTypeRecord>();
    public IEnumerable<MimeTypeRecord> MimeTypeToFileExtensions { get; init; } = Enumerable.Empty<MimeTypeRecord>();
}