using Tenjin.MimeTypes.Generator.Constants;
using Tenjin.MimeTypes.Generator.Services;

namespace Tenjin.MimeTypes.Generator;

public static class Program
{
    public static void Main()
    {
        ImportData();

        Console.WriteLine("Done.");
    }

    private static void ImportData()
    {
        Console.Write("Importing data...");

        var importer = new JsonImporter();
        var data = importer.Import(JsonFilenameConstants.JsonInputFilename);

        UtilitiesCodeBuilder.Build(data).GetAwaiter().GetResult();

        Console.WriteLine("DONE!");
        Console.WriteLine();
    }
}