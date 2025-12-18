using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Phases.Events;


/// <summary>
/// Raise to check if it is possible to schedule actions 
/// </summary>
public class CanSchedulePlayerActionsEvent : CancelableEvent
{
    public required Entity<PlayerComponent> Player;
}