using System.Threading.Tasks;
using Tenjin.Models.Messaging.Configuration;
using Tenjin.Models.Messaging.Progress;

namespace Tenjin.Interfaces.Messaging.Progress
{
    public interface IProgressPublisher : IPublisher<ProgressEvent>
    {
        IProgressPublisher Configure(ProgressPublisherConfiguration configuration);
        Task Initialise(ulong total);
        Task Tick();
        Task Tick(ulong ticks);
    }
}
