using GameServer.Model.Components;

namespace GameServer.Model.Action.Components;


public sealed class ActionHighlightComponent : Component
{
    public HighlightedLayer[] Layers = [];
}


public struct HighlightedLayer
{
    public HighlightType Type { get ; set; }
    public HighlightPattern Pattern { get; set; }
    public RelativeType Relative { get; set; }
    public HighlightVisibility Visibility { get; set; }
    
    public double? Range  { get; set; }
}

public enum HighlightPattern
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