using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Tests.Services
{
    public class CollectionSystemClockProvider : ISystemClockProvider
    {
        private int _index;

        private readonly IEnumerable<DateTime> _timestamps;

        public CollectionSystemClockProvider(IEnumerable<DateTime> timestamps)
        {
            _timestamps = timestamps;
        }

        public DateTime Now()
        {
            var enumeratedTimestamps = _timestamps.ToList();
            var index = (_index++) % enumeratedTimestamps.Count;

            return enumeratedTimestamps[index];
        }
    }
}
