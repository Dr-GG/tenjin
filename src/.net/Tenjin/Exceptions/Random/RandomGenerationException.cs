using System;

namespace Tenjin.Exceptions.Random;

public class RandomGenerationException : Exception
{
    public RandomGenerationException(string message) : base(message)
    { }
}