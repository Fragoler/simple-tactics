using GameServer.Model.Entities;
using GameServer.Model.EventBus;

namespace GameServer.Model.Map;


public sealed class AfterMapLoadedEvent : BaseEvent
{
    public Entity<MapComponent> Map;
}