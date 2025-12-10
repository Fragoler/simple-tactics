using GameServer.Model.Components;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Components;


public sealed class ScheduledActionComponent : Component
{
    public Coordinates? Target = null;
    public string ActionId = "";
}