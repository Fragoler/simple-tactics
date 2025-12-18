using GameServer.Model.Entities;
using GameServer.Model.EventBus;

namespace GameServer.Model.Map;


/// <summary>
/// Raised when game map has been loaded
/// </summary>
public sealed class MapLoadedEvent : BaseEvent
{
    public Entity<MapComponent> Map;
}