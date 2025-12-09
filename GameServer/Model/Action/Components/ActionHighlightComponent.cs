using GameServer.Model.Components;

namespace GameServer.Model.Action.Components;


public sealed class ActionHighlightComponent : Component
{
    public HighlightedLayer[] Layers = [];
}


public struct HighlightedLayer
{
    public HighlightType Type;
    public Pattern Pattern;
    public RelativeType Relative;
    public HighlightVisibility Visibility;

    public double? Range;
}

public enum Pattern
{ 
    Manhattan,
    Adjacent,
    AdjacentDiagonal,
    Circle,
    Self,
    None,
}

public enum RelativeType
{
    Executor,
    Target
}

public enum HighlightType {
    Selection,
    Movement,
    Damage,
    Heal,
    Buff,
    Debuff,
}

public enum HighlightVisibility {
    Selecting,
    Confirmed,
    Always,
}