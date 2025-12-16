using GameServer.Model.Components;

namespace GameServer.Model.Action.Components;


public sealed class ActionsContainerComponent : Component
{
    public string[] ActionPrototypes { get; set; } = [];
}