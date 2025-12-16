using GameServer.Model.Action.Patterns;
using GameServer.Model.Components;
using GameServer.Model.Prototype;

namespace GameServer.Model.Action.Components;


public sealed class ActionHighlightComponent : Component
{
    public HighlightedLayer[] Layers = [];
}


public struct HighlightedLayer
{
    public HighlightType Type { get; set; }
    public required IPattern Pattern { get; set; }
    public RelativeType Relative { get; set; }
    public HighlightVisibility Visibility { get; set; }
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