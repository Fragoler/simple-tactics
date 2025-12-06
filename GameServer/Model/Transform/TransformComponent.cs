using System.Numerics;
using GameServer.Model.Components;

namespace GameServer.Model.Transform;


public sealed class TransformComponent : Component
{
    public Coordinates Coords = new();
}

public record struct Coordinates(uint X, uint Y)
{
    public uint X = X;
    public uint Y = Y;

    public void Set(Coordinates coords)
    {
        X = coords.X;
        Y = coords.Y;
    }
    
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}