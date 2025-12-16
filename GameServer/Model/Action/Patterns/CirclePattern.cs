using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Patterns;


[YamlType("Circle")]
public sealed class CirclePattern : IRangePattern
{
    public string Name => "Circle";
    
    public double Range { get; set; } = 1.5;

    public bool Validate(Coordinates executor, Coordinates target)
    {
        double dx = (int)executor.X - (int)target.X;
        double dy = (int)executor.Y - (int)target.Y;
        var distance = Math.Sqrt(dx * dx + dy * dy);
        
        return distance <= Range;
    }
}