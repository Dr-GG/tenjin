using System;
using System.Collections.Generic;
using System.Linq;
using Tenjin.Interfaces.Diagnostics;

namespace Tenjin.Tests.Services;

public class CollectionSystemClockProvider(IEnumerable<DateTime> timestamps) : ISystemClockProvider
{
    private int _index;

    public DateTime Now()
    {
        var enumeratedTimestamps = timestamps.ToList();
        var index = _index++ % enumeratedTimestamps.Count;

        return enumeratedTimestamps[index];
    }
}