using GameServer.Model.Components;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Components;


/// <summary>
/// Hold info about entity's prepared action
/// </summary>
public sealed class ScheduledActionComponent : Component
{
    public Coordinates? Target { get; set; } = null;
    public string ActionId { get; set; } = "";
}