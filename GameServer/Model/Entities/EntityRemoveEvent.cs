using GameServer.Model.EventBus;

namespace GameServer.Model.Entities;


/// <summary>
/// Raised before entity will be deleted
/// </summary>
public sealed class EntityRemoveEvent : BaseEvent;