using System;
using Microsoft.Extensions.DependencyInjection;
using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Implementations.Messaging.Publishers;

/// <summary>
/// The default .NET Core dependency injection implementation of the PublisherRegistry class.
/// </summary>
public class DotNetCorePublisherRegistry<TKey, TData> : PublisherRegistry<TKey, TData> where TKey : notnull
{
    /// <summary>
    /// Creates a new instance of the DotNetCorePublisherRegistry class.
    /// </summary>
    public DotNetCorePublisherRegistry(IServiceProvider service)
    {
        var publishers = service.GetServices<IDiscoverablePublisher<TKey, TData>>();

        Register(publishers);
    }
}
