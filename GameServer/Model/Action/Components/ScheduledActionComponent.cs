using GameServer.Model.Components;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Components;


public sealed class ScheduledActionComponent : Component
{
    public Coordinates? Target { get; set; } = null;
    public string ActionId { get; set; } = "";
}