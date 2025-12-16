using GameServer.Model.Games;

namespace GameServer.Model.EventBus;

/// <summary>
/// Simple event. You can subscribe on it
/// </summary>
public interface IBaseEvent 
{
    public Game Game { get; }
}

public abstract class BaseEvent : IBaseEvent
{
    public required Game Game { get; init; }
}