namespace Tenjin.Models.Messaging.Publishers.Progress;

public record ProgressEvent
{
    public ProgressEvent()
    { }

    public ProgressEvent(ulong total) : this(0, total)
    { }

    public ProgressEvent(ulong current, ulong total)
    {
        Current = current;
        Total = total;
    }

    public ulong Current { get; init; }
    public ulong Total { get; init; }
}