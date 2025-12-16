using GameServer.Model.Entities;
using GameServer.Model.EventBus;

namespace GameServer.Model.Map;


public sealed class MapInitEvent : BaseEvent
{
    public Entity<MapComponent> Map;
}