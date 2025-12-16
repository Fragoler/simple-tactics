using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Players.Events;


public class PlayerActionsScheduledEvent : BaseEvent
{
    public required Entity<PlayerComponent> Player;
}