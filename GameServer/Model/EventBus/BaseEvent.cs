using GameServer.Model.Games;

namespace GameServer.Model.EventBus;

/// <summary>
/// Simple event. You can subscribe on it
/// </summary>
public abstract class BaseEvent 
{
    public required Game Game { get; init; }
}