using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


/// <summary>
/// A pattern that includes all adjacent cells vertically and horizontally
/// </summary>
[YamlType("Adjacent")]
public sealed class AdjacentPattern : IPattern
{
    public string Name => "Adjacent";
    
    public bool Validate(Coordinates executor, Coordinates target)
    {
        var dx = Math.Abs((int)executor.X - (int)target.X);
        var dy = Math.Abs((int)executor.Y - (int)target.Y);
        
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }
}