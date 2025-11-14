using System.Text;
using Tenjin.Extensions;
using Tenjin.MimeTypes.Generator.Constants;
using Tenjin.MimeTypes.Generator.Models;

namespace Tenjin.MimeTypes.Generator.Services;

public static class UtilitiesCodeBuilder
{
    private const string DefaultFileExtensionsToMimeTypeMapFilename = "FileExtensionToMimeTypeMap.cs";
    private const string DefaultMimeTypeToFileExtensionsMapFilename = "MimeTypeToFileExtensionsMap.cs";
    private const string DefaultTestFileExtensionToMimeTypeTestCasesFilename = "TestFileExtensionToMimeTypeTestCases.cs";
    private const string DefaultTestMimeTypeToFileExtensionTestCasesFilename = "TestMimeTypeToFileExtensionTestCases.cs";
    private const string DefaultTestMimeTypeToFileExtensionsTestCasesFilename = "TestMimeTypeToFileExtensionsTestCases.cs";

    public static async Task Build(
        MimeTypeData data,
        string fileExtensionsToMimeTypeMapFilename = DefaultFileExtensionsToMimeTypeMapFilename,
        string mimeTypeToFileExtensionsMapFilename = DefaultMimeTypeToFileExtensionsMapFilename,
        string testFileExtensionToMimeTypeTestCasesFilename = DefaultTestFileExtensionToMimeTypeTestCasesFilename,
        string testMimeTypeToFileExtensionTestCasesFilename = DefaultTestMimeTypeToFileExtensionTestCasesFilename,
        string testMimeTypeToFileExtensionsTestCasesFilename = DefaultTestMimeTypeToFileExtensionsTestCasesFilename)
    {
        await WriteFileExtensionToMimeTypeMapCode(data, fileExtensionsToMimeTypeMapFilename);
        await WriteMimeTypeToFileExtensionMapCode(data, mimeTypeToFileExtensionsMapFilename);
        await WriteTestFileExtensionToMimeTypeTestCasesCode(data, testFileExtensionToMimeTypeTestCasesFilename);
        await WriteTestMimeTypeToFileExtensionTestCasesCode(data, testMimeTypeToFileExtensionTestCasesFilename);
        await WriteTestMimeTypeToFileExtensionsTestCasesCode(data, testMimeTypeToFileExtensionsTestCasesFilename);
    }

    private static async Task WriteFileExtensionToMimeTypeMapCode(MimeTypeData data, string path)
    {
        var code = GetFileExtensionToMimeTypeMapCode(data.FileExtensionToMimeTypes);

        await File.WriteAllTextAsync(path, code);
    }

    private static string GetFileExtensionToMimeTypeMapCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var items = GetFileExtensionToMimeTypeMapItemsCode(mimeTypes);

        return CodeConstants.PopulateFileExtensionToMimeTypeMapMethodTemplate
            .Replace(CodeConstants.CodeSnippet, items);
    }

    private static string GetFileExtensionToMimeTypeMapItemsCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var output = new StringBuilder();
        var fileExtensions = mimeTypes
            .OrderBy(m => m.FileExtension);

        foreach (var mimeType in fileExtensions)
        {
            output.Append(GetFileExtensionToMimeTypeMapItemCode(mimeType));
        }

        return output.ToString();
    }

    private static string GetFileExtensionToMimeTypeMapItemCode(MimeTypeRecord mimeType)
    {
        return CodeConstants.PopulateFileExtensionToMimeTypeMapItemTemplate
            .Replace(CodeConstants.MimeTypeSnippet, mimeType.MimeType)
            .Replace(CodeConstants.FileExtensionSnippet, mimeType.FileExtension);
    }

    private static async Task WriteMimeTypeToFileExtensionMapCode(MimeTypeData data, string path)
    {
        var code = GetMimeTypeToFileExtensionsMapCode(data.MimeTypeToFileExtensions);

        await File.WriteAllTextAsync(path, code);
    }

    private static string GetMimeTypeToFileExtensionsMapCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var dictionaryItemsCode = GetMimeTypeToFileExtensionsMapItemsCode(mimeTypes);

        return CodeConstants.PopulateMimeTypeToFileExtensionsMapMethodsTemplate
            .Replace(CodeConstants.CodeSnippet, dictionaryItemsCode);
    }

    private static string GetMimeTypeToFileExtensionsMapItemsCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var output = new StringBuilder();

        foreach (var mimeType in mimeTypes)
        {
            var code = GetMimeTypeToFileExtensionsMapItemCode(mimeType);

            output.Append(code);
        }

        return output.ToString();
    }

    private static string GetMimeTypeToFileExtensionsMapItemCode(MimeTypeRecord mimeType)
    {
        var arrayDeclaration = GetMimeTypeToFileExtensionsMapArrayItemCode(mimeType);

        return CodeConstants.PopulateMimeTypesToFileExtensionsMapItemTemplate
            .Replace(CodeConstants.MimeTypeSnippet, mimeType.MimeType)
            .Replace(CodeConstants.CodeSnippet, arrayDeclaration);
    }

    private static string GetMimeTypeToFileExtensionsMapArrayItemCode(MimeTypeRecord mimeType)
    {
        if (mimeType.FileExtensions.IsEmpty())
        {
            return string.Empty;
        }

        var fileExtensions = mimeType
            .FileExtensions
            .Select(f => $"\"{f}\"")
            .ToList();

        return string.Join(", ", fileExtensions);
    }

    private static async Task WriteTestFileExtensionToMimeTypeTestCasesCode(
        MimeTypeData data,
        string path)
    {
        var code = GetTestFileExtensionToMimeTypeTestCasesCode(data.MimeTypeToFileExtensions);

        await File.WriteAllTextAsync(path, code);
    }

    private static string GetTestFileExtensionToMimeTypeTestCasesCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var output = new StringBuilder();

        foreach (var mimeType in mimeTypes)
        {
            output.Append(GetTestFileExtensionToMimeTypeTestCasesCode(mimeType));
        }

        return output.ToString();
    }

    private static string GetTestFileExtensionToMimeTypeTestCasesCode(MimeTypeRecord mimeType)
    {
        var output = new StringBuilder();

        foreach (var fileExtension in mimeType.FileExtensions)
        {
            output.Append(GetTestFileExtensionToMimeTypeTestCasesCode(mimeType.MimeType, fileExtension));
        }

        return output.ToString();
    }

    private static string GetTestFileExtensionToMimeTypeTestCasesCode(string mimeType, string fileExtension)
    {
        var output = new StringBuilder();
        var uppercaseWithPoint = fileExtension.ToUpper();
        var lowercaseWithoutPoint = fileExtension.Replace(".", string.Empty);
        var uppercaseWithoutPoint = uppercaseWithPoint.Replace(".", string.Empty);

        output.Append(GetTestFileExtensionToMimeTypeTestCaseCode(mimeType, fileExtension));
        output.Append(GetTestFileExtensionToMimeTypeTestCaseCode(mimeType, uppercaseWithPoint));
        output.Append(GetTestFileExtensionToMimeTypeTestCaseCode(mimeType, lowercaseWithoutPoint));
        output.Append(GetTestFileExtensionToMimeTypeTestCaseCode(mimeType, uppercaseWithoutPoint));

        return output.ToString();
    }

    private static string GetTestFileExtensionToMimeTypeTestCaseCode(string mimeType, string fileExtension)
    {
        return CodeConstants.TestFileExtensionToMimeTypeTestCaseTemplate
            .Replace(CodeConstants.MimeTypeSnippet, mimeType)
            .Replace(CodeConstants.FileExtensionSnippet, fileExtension);
    }

    private static async Task WriteTestMimeTypeToFileExtensionTestCasesCode(
        MimeTypeData data,
        string path)
    {
        var code = GetTestMimeTypeToFileExtensionTestCasesCode(data.MimeTypeToFileExtensions);

        await File.WriteAllTextAsync(path, code);
    }

    private static string GetTestMimeTypeToFileExtensionTestCasesCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var output = new StringBuilder();

        foreach (var mimeType in mimeTypes)
        {
            output.Append(GetTestMimeTypeToFileExtensionTestCasesCode(mimeType));
        }

        return output.ToString();
    }

    private static string GetTestMimeTypeToFileExtensionTestCasesCode(MimeTypeRecord mimeType)
    {
        return GetTestMimeTypeToFileExtensionTestCasesCode(mimeType.MimeType, mimeType.FileExtensions.First());
    }

    private static string GetTestMimeTypeToFileExtensionTestCasesCode(string mimeType, string fileExtension)
    {
        var output = new StringBuilder();

        output.Append(GetTestMimeTypeToFileExtensionTestCaseCode(mimeType, fileExtension));
        output.Append(GetTestMimeTypeToFileExtensionTestCaseCode(mimeType.ToUpper(), fileExtension));

        return output.ToString();
    }

    private static string GetTestMimeTypeToFileExtensionTestCaseCode(string mimeType, string fileExtension)
    {
        return CodeConstants.TestMimeTypeToFileExtensionTestCaseTemplate
            .Replace(CodeConstants.MimeTypeSnippet, mimeType)
            .Replace(CodeConstants.FileExtensionSnippet, fileExtension);
    }

    private static async Task WriteTestMimeTypeToFileExtensionsTestCasesCode(
        MimeTypeData data,
        string path)
    {
        var code = GetTestMimeTypeToFileExtensionsTestCasesCode(data.MimeTypeToFileExtensions);

        await File.WriteAllTextAsync(path, code);
    }

    private static string GetTestMimeTypeToFileExtensionsTestCasesCode(IEnumerable<MimeTypeRecord> mimeTypes)
    {
        var output = new StringBuilder();

        foreach (var mimeType in mimeTypes)
        {
            output.Append(GetTestMimeTypeToFileExtensionsTestCasesCode(mimeType));
        }

        return output.ToString();
    }

    private static string GetTestMimeTypeToFileExtensionsTestCasesCode(MimeTypeRecord mimeType)
    {
        return GetTestMimeTypeToFileExtensionsTestCasesCode(mimeType.MimeType, mimeType.FileExtensions);
    }

    private static string GetTestMimeTypeToFileExtensionsTestCasesCode(string mimeType, IEnumerable<string> fileExtensions)
    {
        var output = new StringBuilder();
        var enumeratedFileExtensions = fileExtensions.ToArray();

        output.Append(GetTestMimeTypeToFileExtensionsTestCaseCode(mimeType, enumeratedFileExtensions));
        output.Append(GetTestMimeTypeToFileExtensionsTestCaseCode(mimeType.ToUpper(), enumeratedFileExtensions));

        return output.ToString();
    }

    private static string GetTestMimeTypeToFileExtensionsTestCaseCode(string mimeType, IEnumerable<string> fileExtensions)
    {
        var output = new StringBuilder();

        foreach (var fileExtension in fileExtensions)
        {
            output.Append($"\"{fileExtension}\", ");
        }

        return CodeConstants.TestMimeTypeToFileExtensionsTestCaseTemplate
            .Replace(CodeConstants.MimeTypeSnippet, mimeType)
            .Replace(CodeConstants.CodeSnippet, output.ToString());
    }
}