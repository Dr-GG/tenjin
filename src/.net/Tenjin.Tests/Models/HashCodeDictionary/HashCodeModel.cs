namespace Tenjin.Tests.Models.HashCodeDictionary;

public class HashCodeModel
{
    public int Property1 { get; init; }

    public int Property2 { get; init; }

    public override int GetHashCode()
    {
        return Property1 * 4799 ^ Property2 * 20707;
    }

    public override bool Equals(object? obj)
    {
        return obj is HashCodeModel model
            && Property1 == model.Property1
            && Property2 == model.Property2;
    }
}