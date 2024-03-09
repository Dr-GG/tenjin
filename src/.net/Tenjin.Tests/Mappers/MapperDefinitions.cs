using System.Globalization;
using JetBrains.Annotations;
using Tenjin.Interfaces.Mappers;

namespace Tenjin.Tests.Mappers;

[UsedImplicitly]
public abstract class AbstractUnaryMapper : IUnaryMapper<string, int>
{
    public abstract int Map(string value);
}

[UsedImplicitly]
public abstract class AbstractBinaryMapper : IBinaryMapper<string, int>
{
    public abstract string Map(int source);
    public abstract int Map(string source);
}

public class ComplexStringUnaryMapper : IUnaryMapper<string, int>,
                                        IUnaryMapper<string, double>,
                                        IUnaryMapper<string, bool>
{
    int IUnaryMapper<string, int>.Map(string value)
    {
        return int.Parse(value);
    }

    double IUnaryMapper<string, double>.Map(string value)
    {
        return double.Parse(value, CultureInfo.InvariantCulture);
    }

    bool IUnaryMapper<string, bool>.Map(string value)
    {
        return bool.Parse(value);
    }
}

public class ComplexIntegerUnaryMapper : IUnaryMapper<short, int>,
                                         IUnaryMapper<long, int>
{
    public int Map(short value)
    {
        return value;
    }

    public int Map(long value)
    {
        return (int)value;
    }
}

public class ComplexStringBinaryMapper : IBinaryMapper<string, int>,
                                         IBinaryMapper<string, double>,
                                         IBinaryMapper<string, bool>
{
    public string Map(int source)
    {
        return source.ToString();
    }

    public string Map(double source)
    {
        return source.ToString(CultureInfo.InvariantCulture);
    }

    public string Map(bool source)
    {
        return source.ToString();
    }

    bool IBinaryMapper<string, bool>.Map(string source)
    {
        return bool.Parse(source);
    }

    double IBinaryMapper<string, double>.Map(string source)
    {
        return double.Parse(source);
    }

    int IBinaryMapper<string, int>.Map(string source)
    {
        return int.Parse(source);
    }
}
