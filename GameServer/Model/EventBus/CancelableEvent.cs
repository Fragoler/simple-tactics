using GameServer.Model.Games;

namespace GameServer.Model.EventBus;


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
