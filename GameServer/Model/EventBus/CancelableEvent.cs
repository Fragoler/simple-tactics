using GameServer.Model.Games;

namespace GameServer.Model.EventBus;

/// <summary>
/// An Event which is could be cancelled
/// Use to prevent execution of something
/// </summary>
public interface ICancelableEvent : IBaseEvent
{
    bool Cancelled { get; }

    void Cancel();
}

public abstract class CancelableEvent : BaseEvent, ICancelableEvent
{
    public bool Cancelled { get; private set; } = false;
    public void Cancel()
    {
        Cancelled = true;
    }
}
