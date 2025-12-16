using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Phases.Events;

public class CanSchedulePlayerActionsEvent : CancelableEvent
{
    public required Entity<PlayerComponent> Player;
}