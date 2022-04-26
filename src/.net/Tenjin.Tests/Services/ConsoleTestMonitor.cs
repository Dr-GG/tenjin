using System;
using System.IO;
using System.Text;

namespace Tenjin.Tests.Services
{
    public class ConsoleTestMonitor : IDisposable
    {
        private bool _disposed;

        private readonly StringBuilder _output;
        private readonly TextWriter _originalConsoleStream;

        public ConsoleTestMonitor()
        {
            _output = new StringBuilder();
            _originalConsoleStream = Console.Out;

            var writer = new StringWriter(_output);

            Console.SetOut(writer);
        }

        public string GetOutputText()
        {
            var result = _output.ToString();

            Dispose();

            return result;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            Console.SetOut(_originalConsoleStream);
        }
    }
}
