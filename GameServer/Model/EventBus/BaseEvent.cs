using GameServer.Model.Games;

namespace GameServer.Model.EventBus;

/// <summary>
/// Простое событие. На него можно подписаться
/// </summary>
public abstract class BaseEvent 
{
    public required Game Game { get; init;  }
}