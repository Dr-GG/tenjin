namespace Tenjin.Interfaces.Messaging
{
    public interface IDiscoverablePublisher<out TKey, TData> : IPublisher<TData>
    {
        TKey Id { get; }
    }
}
