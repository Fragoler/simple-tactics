using GameServer.Model.Components;

namespace GameServer.Model.Action.Components;


/// <summary>
/// Entity with this comp can execute actions
/// </summary>
public sealed class ActionsContainerComponent : Component
{
    public string[] ActionPrototypes { get; set; } = [];
}