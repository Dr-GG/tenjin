using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tenjin.Extensions;

public static class ObjectExtensions
{
    public const string DefaultHeadingUnderline = "=";

    public static bool DoesNotEqual<TObject>(this TObject? left, TObject? right)
    {
        return !Equals(left, right);
    }

    public static bool EqualsAll(this object? root, params object?[] objects)
    {
        return objects.All(obj => Equals(root, obj));
    }

    public static bool EqualsAny(this object? root, params object?[] objects)
    {
        return objects.Any(obj => Equals(root, obj));
    }

    public static bool DoesNotEqualAll(this object? root, params object?[] objects)
    {
        return objects.All(obj => !Equals(root, obj));
    }

    public static bool DoesNotEqualAny(this object? root, params object?[] objects)
    {
        return objects.Any(obj => !Equals(root, obj));
    }

    /*
     * This methods exists because the C# compiler does not allow the following code:
     *
     * someData.Select(data => () => SomeMethodThatReturnsATask());
     *
     * The C# compiler complains that it cannot determine the return type efficiently.
     *
     * Therefore, the following code does work:
     *
     * someData.Select(data => new Func<Task>(() => SomeMethodThatReturnsATask());
     *
     * However, the Resharper, and the C# compiler warns that the explicit cast is redundant and can be dropped.
     * To circumvent this, the method exists to just cast, the code () => SomeMethodThatReturnsATask() to a Func<Task> type.
     */
    public static Func<Task> ToFunctionTask(this object _, Func<Task> function)
    {
        return function;
    }

    public static void WriteLines(this object? _, int numberOfWriteLines = 1)
    {
        InternalWriteLines(numberOfWriteLines);
    }

    public static void Write(
        this IFormattable? root,
        string? format = null,
        IFormatProvider? formatProvider = null)
    {
        var output = root?.ToString(format, formatProvider) ?? string.Empty;

        Console.Write(output);
    }

    public static void WriteLine(
        this IFormattable? root,
        string? format = null,
        IFormatProvider? formatProvider = null,
        int? writeLineAppends = null)
    {
        var output = root?.ToString(format, formatProvider) ?? string.Empty;

        InternalWriteLine(output, writeLineAppends);
    }

    public static void Write(this object? root)
    {
        var output = root?.ToString() ?? string.Empty;

        Console.Write(output);
    }

    public static void WriteLine(
        this object? root,
        int? writeLineAppends = null)
    {
        var output = root?.ToString() ?? string.Empty;

        InternalWriteLine(output, writeLineAppends);
    }

    public static void WriteHeading(
        this object? root,
        string? headingUnderline,
        int? writeLineAppends = null)
    {
        var output = root?.ToString() ?? string.Empty;
        var underline = headingUnderline.IsNullOrEmpty()
            ? DefaultHeadingUnderline
            : headingUnderline;

        InternalWriteHeading(output, underline, writeLineAppends);
    }

    public static void WriteHeading(
        this IFormattable? root,
        string? format = null,
        IFormatProvider? formatProvider = null,
        string? headingUnderline = null,
        int? writeLineAppends = null)
    {
        var output = root?.ToString(format, formatProvider) ?? string.Empty;
        var underline = headingUnderline.IsNullOrEmpty()
            ? DefaultHeadingUnderline
            : headingUnderline;

        InternalWriteHeading(output, underline, writeLineAppends);
    }

    private static void InternalWriteHeading(
        string? value,
        string headingUnderline,
        int? writeLineAppends)
    {
        Console.WriteLine(value);

        InternalWriteHeadingUnderline(value, headingUnderline);
        InternalWriteLines(writeLineAppends);
    }

    private static void InternalWriteHeadingUnderline(string? value, string headingUnderline)
    {
        if (value.IsNullOrEmpty())
        {
            return;
        }

        for (var i = 0; i < value.Length; i++)
        {
            Console.Write(headingUnderline);
        }

        Console.WriteLine();
    }

    private static void InternalWriteLine(string? value, int? writeLineAppends = null)
    {
        Console.WriteLine(value);

        InternalWriteLines(writeLineAppends);
    }

    private static void InternalWriteLines(int? numberOfWriteLines)
    {
        if (numberOfWriteLines == null)
        {
            return;
        }

        for (var i = 0; i < numberOfWriteLines; i++)
        {
            Console.WriteLine();
        }
    }
}