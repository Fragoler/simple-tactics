using GameServer.Model.EventBus;

namespace GameServer.Model.Phases.Events;


/// <summary>
/// Raised to check if actions can be executed
/// </summary>
public class CanEndPlaningPhaseEvent : CancelableEvent
{
}