using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Players.Events;


/// <summary>
/// Raised when player schedule actions
/// </summary>
public class PlayerActionsScheduledEvent : BaseEvent
{
    public required Entity<PlayerComponent> Player;
}