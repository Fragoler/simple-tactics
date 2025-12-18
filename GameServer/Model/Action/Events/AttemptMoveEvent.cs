using GameServer.Model.EventBus;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Events;


/// <summary>
/// Request when entity tries move
/// </summary>
public sealed class AttemptMoveEvent : CancelableEvent
{
    public required Coordinates From;
    public required Coordinates To;
}