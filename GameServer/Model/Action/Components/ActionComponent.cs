using GameServer.Model.Action.Effects;
using GameServer.Model.Components;

namespace GameServer.Model.Action.Components;


public sealed class ActionComponent : Component
{
    public string Name = "";
    public string Icon = "X";

    public ActionTargetType TargetType = ActionTargetType.None;
    public TargetFilter TargetFilter = new();
    
    
    public string EffectId = "None";
    public IActionEffect? Effect;
}

public enum ActionTargetType
{
    Cell,
    None
}

public struct TargetFilter
{
    public Pattern Pattern = Pattern.None;
    public double Range = 1.5;
    
    public bool RequiredEnemy = false;
    public bool RequiredAlly =  false;
    public bool RequiredFreeSpace = false;
    public uint MaxTargets = 1;

    public TargetFilter()
    {
    }
}