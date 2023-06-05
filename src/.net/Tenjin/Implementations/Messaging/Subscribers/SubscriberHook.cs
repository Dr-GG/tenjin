using System;
using System.Threading.Tasks;
using Tenjin.Enums.Messaging;
using Tenjin.Interfaces.Messaging.Publishers;
using Tenjin.Interfaces.Messaging.Subscribers;
using Tenjin.Models.Messaging.Publishers;

namespace Tenjin.Implementations.Messaging.Subscribers;

/// <summary>
/// The default implementation of the ISubscriberHook interface.
/// </summary>
public class SubscriberHook<TData> : ISubscriberHook<TData>
{
    private IPublisherLock? _lock;

    private readonly Func<PublishEvent<TData>, Task> _onNextAction;
    private readonly Func<PublishEvent<TData>, Task>? _onDisposeAction;
    private readonly Func<PublishEvent<TData>, Task>? _onErrorAction;

    /// <summary>
    /// Creates a new instance with the appropriate callbacks/hooks.
    /// </summary>
    public SubscriberHook(
        object parent,
        Func<PublishEvent<TData>, Task> onNextAction,
        Func<PublishEvent<TData>, Task>? onDisposeAction = null,
        Func<PublishEvent<TData>, Task>? onErrorAction = null) :
        this(
            parent.GetHashCode().ToString(),
            onNextAction, onDisposeAction, onErrorAction
        )
    { }

    /// <summary>
    /// Creates a new instance with the appropriate callbacks/hooks.
    /// </summary>
    public SubscriberHook(
        string id,
        Func<PublishEvent<TData>, Task> onNextAction,
        Func<PublishEvent<TData>, Task>? onDisposeAction = null,
        Func<PublishEvent<TData>, Task>? onErrorAction = null)
    {
        Id = id;
        _onNextAction = onNextAction;
        _onDisposeAction = onDisposeAction;
        _onErrorAction = onErrorAction;
    }

    /// <inheritdoc />
    public string Id { get; }

    public async Task<ISubscriberHook<TData>> Subscribe(IPublisher<TData> publisher)
    {
        if (_lock != null)
        {
            throw new InvalidOperationException("Subscriber hook already has a publisher.");
        }

        _lock = await publisher.Subscribe(this);

        return this;
    }

    /// <inheritdoc />
    public async Task Receive(PublishEvent<TData> publishEvent)
    {
        switch (publishEvent.Type)
        {
            case PublishEventType.Publish:
                await ExecuteAction(publishEvent, _onNextAction);
                break;

            case PublishEventType.Disposing:
                await ExecuteAction(publishEvent, _onDisposeAction);
                break;

            case PublishEventType.Error:
                await ExecuteAction(publishEvent, _onErrorAction);
                break;

            default:
                throw new NotSupportedException(
                    $"No action relay for publish event type {publishEvent.Type}.");
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Dispose(true);

        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        _lock?.Dispose();
    }

    private static Task ExecuteAction(
    PublishEvent<TData> publishEvent,
    Func<PublishEvent<TData>, Task>? action)
    {
        return action == null
            ? Task.CompletedTask
            : action(publishEvent);
    }
}