using System;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Implementations.Diagnostics
{
    public class SystemClockProvider : ISystemClockProvider
    {
        private readonly bool _useUtc;

        public SystemClockProvider(bool useUtc)
        {
            _useUtc = useUtc;
        }

        public DateTime Now()
        {
            return _useUtc ? DateTime.UtcNow : DateTime.Now;
        }
    }
}
