using System;
using Tenjin.Enums.Messaging;
using Tenjin.Interfaces.Messaging.Publishers;

namespace Tenjin.Models.Messaging.Publishers;

public record PublishEvent<TData>
{
    public PublishEvent()
    { }

    public PublishEvent(IPublisher<TData> source, TData data)
    {
        Source = source;
        Data = data;
    }

    public PublishEvent(IPublisher<TData> source, Exception error)
    {
        Source = source;
        Error = error;
        Type = PublishEventType.Error;
    }

    public PublishEvent(IPublisher<TData> source, PublishEventType type)
    {
        Source = source;
        Type = type;
    }

    public PublishEventType Type { get; init; } = PublishEventType.Publish;
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreateTimestamp { get; init; } = DateTime.UtcNow;
    public DateTime DispatchTimestamp { get; set; }
    public IPublisher<TData> Source { get; init; } = null!;
    public Exception? Error { get; init; }
    public TData? Data { get; init; }
}