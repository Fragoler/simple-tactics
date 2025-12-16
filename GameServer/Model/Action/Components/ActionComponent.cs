using GameServer.Model.Action.Effects;
using GameServer.Model.Action.Patterns;
using GameServer.Model.Components;
using GameServer.Model.Prototype;

namespace GameServer.Model.Action.Components;


public sealed class ActionComponent : Component
{
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "X";

    public ulong Pace  { get; set; } = 10;
    
    public required IActionEffect Effect { get; set; }
    public TargetFilter TargetFilter { get; set; }
}

public enum ActionTargetType
{
    Cell,
    None
}

[YamlType("TargetFilter")]
public struct TargetFilter()
{
    public required IPattern Pattern { get; set; }
    
    public bool RequiredEnemy { get; set; } = false;
    public bool RequiredAlly { get; set; } =  false;
    public bool RequiredFreeSpace { get; set; } = false;
}