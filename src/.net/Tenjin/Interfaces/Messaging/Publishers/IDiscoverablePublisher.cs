namespace Tenjin.Interfaces.Messaging.Publishers;

public interface IDiscoverablePublisher<out TKey, TData> : IPublisher<TData>
{
    TKey Id { get; }
}