using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


/// <summary>
/// A pattern that includes all cells that can be reached in a fixed number of vertical and horizontal movements
/// </summary>
[YamlType("Manhattan")]
public sealed class ManhattanPattern : IRangePattern
{
    public string Name => "Manhattan";
    
    public double Range { get; set; } = 1;

    public bool Validate(Coordinates executor, Coordinates target)
    {
        var distance = Math.Abs((int)executor.X - (int)target.X) + 
                       Math.Abs((int)executor.Y - (int)target.Y);
        return distance <= Range;
    }
}