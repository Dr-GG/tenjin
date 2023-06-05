using System;
using System.Threading.Tasks;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for the Object class.
/// </summary>
public static class ObjectExtensions
{
    public const string DefaultHeadingUnderline = "=";

    /// <summary>
    /// Determines if two object instances do not equal one another.
    /// </summary>
    public static bool DoesNotEqual<TObject>(this TObject? left, TObject? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Determines if an object instance equals all object instances provided.
    /// </summary>
    public static bool EqualsAll(this object? root, params object?[] objects)
    {
        return Array.TrueForAll(objects, obj => Equals(root, obj));
    }

    /// <summary>
    /// Determines if an object instance equals any of the object instances provided.
    /// </summary>
    public static bool EqualsAny(this object? root, params object?[] objects)
    {
        return Array.Exists(objects, obj => Equals(root, obj));
    }

    /// <summary>
    /// Determines if an object instance does not equal all object instances provided.
    /// </summary>
    public static bool DoesNotEqualAll(this object? root, params object?[] objects)
    {
        return Array.TrueForAll(objects, obj => !Equals(root, obj));
    }

    /// <summary>
    /// Determines if an object instance does not equal any object instances provided.
    /// </summary>
    public static bool DoesNotEqualAny(this object? root, params object?[] objects)
    {
        return Array.Exists(objects, obj => !Equals(root, obj));
    }

    /// <summary>
    /// Casts an object to a simple Func with a Task return type.
    /// </summary>
    /// <remarks>
    /// This methods exists because the C# compiler does not allow the following code:
    ///     someData.Select(data => () => SomeMethodThatReturnsATask());
    /// 
    /// The C# compiler complains that it cannot determine the return type efficiently.
    /// 
    /// Therefore, the following code does work:
    ///     someData.Select(data => new Func<Task>(() => SomeMethodThatReturnsATask());
    ///     
    /// However, some static analysis tools such as Resharper, and the C# compiler warns that the explicit cast is redundant and can be dropped.
    /// To circumvent this, the method exists to cast, the code() => SomeMethodThatReturnsATask() to a Func<Task> type.
    /// </remarks>
    public static Func<Task> ToFunctionTask(this object _, Func<Task> function)
    {
        return function;
    }

    /// <summary>
    /// An extension method that output the object to the Console instance with a new line terminator.
    /// </summary>
    public static void WriteLines(this object? _, int numberOfWriteLines = 1)
    {
        InternalWriteLines(numberOfWriteLines);
    }

    /// <summary>
    /// An extension method that output an IFormattable to the Console instance with a specified format and a new line terminator.
    /// </summary>
    public static void Write(
        this IFormattable? root,
        string? format = null,
        IFormatProvider? formatProvider = null)
    {
        var output = root?.ToString(format, formatProvider) ?? string.Empty;

        Console.Write(output);
    }

    /// <summary>
    /// An extension method that output an Object to the Console instance with a specified format without a new line terminator.
    /// </summary>
    public static void Write(this object? root)
    {
        var output = root?.ToString() ?? string.Empty;

        Console.Write(output);
    }

    /// <summary>
    /// An extension method that output an IFormattable to the Console instance with a specified format, IFormatProvider and a new line terminator.
    /// </summary>
    public static void WriteLine(
        this IFormattable? root,
        string? format = null,
        IFormatProvider? formatProvider = null,
        int? writeLineAppends = null)
    {
        var output = root?.ToString(format, formatProvider) ?? string.Empty;

        InternalWriteLine(output, writeLineAppends);
    }

    /// <summary>
    /// An extension method that output an Object to the Console instance with a specified format.
    /// </summary>
    public static void WriteLine(
        this object? root,
        int? writeLineAppends = null)
    {
        var output = root?.ToString() ?? string.Empty;

        InternalWriteLine(output, writeLineAppends);
    }

    /// <summary>
    /// An extension method that output an Object to the Console instance as a heading with a heading underlining character.
    /// </summary>
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

    /// <summary>
    /// An extension method that output an IFormattable to the Console instance as a heading with a heading underlining character, specified format and IFormatProvider.
    /// </summary>
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