using System;
using System.Threading.Tasks;

namespace Tenjin.Tests.Utilities
{
    public static class DisposeUtilities
    {
        /*
         * The Dispose and DisposeAsync methods exist to prevent Resharper and C# compiler warnings.
         *
         * In some test cases, the Dispose method of an object is tested.
         * However, the test method is async, and therefore the compiler complains that an async version is available.
         *
         * These methods allow for the warnings to be ignored.
         */
        public static void Dispose(IDisposable disposable)
        {
            disposable.Dispose();
        }

        public static async Task DisposeAsync(IAsyncDisposable disposable)
        {
            await disposable.DisposeAsync();
        }
    }
}
