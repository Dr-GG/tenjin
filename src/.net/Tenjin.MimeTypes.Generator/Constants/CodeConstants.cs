namespace Tenjin.MimeTypes.Generator.Constants;

public static class CodeConstants
{
    public const string CodeSnippet = "CODE_SNIPPET";
    public const string MimeTypeSnippet = "MIME_TYPE";
    public const string FileExtensionSnippet = "FILE_EXTENSION";

    public const string PopulateFileExtensionToMimeTypeMapMethodTemplate =
        "private static IDictionary<string, string> PopulateFileExtensionToMimeTypeMap()\r\n" +
        "{\r\n" +
        "    return new Dictionary<string, string>\r\n" +
        "    {\r\n" +
        "CODE_SNIPPET" +
        "    };\r\n" +
        "}\r\n";

    public const string PopulateFileExtensionToMimeTypeMapItemTemplate =
        "        { \"FILE_EXTENSION\", \"MIME_TYPE\" },\r\n";

    public const string PopulateMimeTypeToFileExtensionsMapMethodsTemplate =
        "private static IDictionary<string, IEnumerable<string>> PopulateMimeTypeToFileExtensionsMap()\r\n" +
        "{\r\n" +
        "    return new Dictionary<string, IEnumerable<string>>\r\n" +
        "    {\r\n" +
        "CODE_SNIPPET" +
        "    };\r\n" +
        "}";

    public const string PopulateMimeTypesToFileExtensionsMapItemTemplate = "             { \"MIME_TYPE\", new[] { CODE_SNIPPET } }, \r\n";
    public const string TestFileExtensionToMimeTypeTestCaseTemplate = "[TestCase(\"FILE_EXTENSION\", \"MIME_TYPE\")]\r\n";
    public const string TestMimeTypeToFileExtensionTestCaseTemplate = "[TestCase(\"MIME_TYPE\", \"FILE_EXTENSION\")]\r\n";
    public const string TestMimeTypeToFileExtensionsTestCaseTemplate = "[TestCase(\"MIME_TYPE\", new[] { CODE_SNIPPET })]\r\n";
}