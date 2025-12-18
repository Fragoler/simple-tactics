using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Phases.Events;


/// <summary>
/// Raised when actions' execution has been completed
/// </summary>
public class EndExecutingPhaseEvent : BaseEvent {}