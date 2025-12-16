using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


[YamlType("Self")]
public sealed class SelfPattern : IPattern
{
    public string Name => "Self";
    
    public bool Validate(Coordinates executor, Coordinates target)
    {
        return executor == target;
    }
}

