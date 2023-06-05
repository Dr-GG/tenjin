using System.Threading;

namespace Tenjin.Extensions;

/// <summary>
/// A collection of extension methods for CancellationToken instances.
/// </summary>
public static class CancellationTokenExtensions
{
    /// <summary>
    /// Determines if a CancellationToken can continue.
    /// </summary>
    public static bool CanContinue(this CancellationToken cancellationToken)
    {
        return !cancellationToken.IsCancellationRequested;
    }
}