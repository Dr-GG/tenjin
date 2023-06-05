using System.Threading.Tasks;
using Tenjin.Models.Messaging.Publishers.Configuration;
using Tenjin.Models.Messaging.Publishers.Progress;

namespace Tenjin.Interfaces.Messaging.Publishers.Progress;

/// <summary>
/// The extended IPublisher interface that provides incremental progress updates.
/// </summary>
public interface IProgressPublisher<TProgressEvent> : IPublisher<TProgressEvent>
    where TProgressEvent : ProgressEvent
{
    /// <summary>
    /// Configures the IProgressPublisher via a ProgressPublisherConfiguration instance.
    /// </summary>
    IProgressPublisher<TProgressEvent> Configure(ProgressPublisherConfiguration configuration);

    /// <summary>
    /// Resets or initialises the IProgressPublisher to a specific point.
    /// </summary>
    Task Initialise(ulong total, bool publish = true);

    /// <summary>
    /// Executes one progress tick.
    /// </summary>
    Task Tick();

    /// <summary>
    /// Executes a progress tick of a specified value.
    /// </summary>
    Task Tick(ulong ticks);
}