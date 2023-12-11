namespace Tenjin.Tests.Models.Console;

public class ConsoleObject(string name)
{
    public static string GetOutputText(string name)
    {
        return $"Hello tests, my name is {name}";
    }

    public string Name { get; } = name;

    public override string ToString()
    {
        return GetOutputText(Name);
    }
}