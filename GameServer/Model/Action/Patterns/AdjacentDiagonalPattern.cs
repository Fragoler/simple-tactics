using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


[YamlType("AdjacentDiagonal")]
public sealed class AdjacentDiagonalPattern : IPattern
{
    public string Name => "AdjacentDiagonal";
    
    public bool Validate(Coordinates executor, Coordinates target)
    {
        var dx = Math.Abs((int)executor.X - (int)target.X);
        var dy = Math.Abs((int)executor.Y - (int)target.Y);
        
        return dx <= 1 && dy <= 1 && (dx != 0 || dy != 0);
    }
}