namespace Tenjin.Tests.Models.Console
{
    public class ConsoleObject
    {
        public static string GetOutputText(string name)
        {
            return $"Hello tests, my name is {name}";
        }

        public string Name { get; }

        public ConsoleObject(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return GetOutputText(Name);
        }
    }
}
