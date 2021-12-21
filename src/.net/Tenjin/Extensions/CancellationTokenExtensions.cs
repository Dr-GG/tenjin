using System.Threading;

namespace Tenjin.Extensions
{
    public static class CancellationTokenExtensions
    {
        public static bool CanContinue(this CancellationToken cancellationToken)
        {
            return !cancellationToken.IsCancellationRequested;
        }
    }
}
