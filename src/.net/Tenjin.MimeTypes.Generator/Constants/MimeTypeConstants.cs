// ReSharper disable StringLiteralTypo

using Tenjin.MimeTypes.Generator.Models;

namespace Tenjin.MimeTypes.Generator.Constants;

public static class MimeTypeConstants
{
    public static readonly IEnumerable<MimeTypeRecord> PrioritisedMimeTypeFileExtensions = new[]
    {
        new MimeTypeRecord("application/octet-stream", ".bin"),
        new MimeTypeRecord("application/msword", ".doc"),
        new MimeTypeRecord("application/postscript", ".ps"),
        new MimeTypeRecord("application/vnd.ms-excel", ".xls"),
        new MimeTypeRecord("application/vnd.ms-powerpoint", ".ppt"),
        new MimeTypeRecord("application/vnd.visio", ".vsd"),
        new MimeTypeRecord("application/xml", ".xml"),
        new MimeTypeRecord("audio/midi", ".midi"),
        new MimeTypeRecord("audio/mpeg", ".mp3"),
        new MimeTypeRecord("audio/ogg", ".ogg"),
        new MimeTypeRecord("audio/x-aiff", ".aiff"),
        new MimeTypeRecord("image/jpeg", ".jpg"),
        new MimeTypeRecord("text/plain", ".txt"),
        new MimeTypeRecord("video/mpeg", ".mpeg"),
        new MimeTypeRecord("video/mp4", ".mp4")
    };
}