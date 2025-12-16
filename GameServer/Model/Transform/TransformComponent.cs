using System.Numerics;
using GameServer.Model.Components;

namespace GameServer.Model.Transform;


public sealed class TransformComponent : Component
{
    public Coordinates Coords { get; set; } = new();
}

public struct Coordinates(uint x, uint y) : IEquatable<Coordinates>
{
    public uint X { get; set; } = x;
    public uint Y { get; set; } = y;

    public static implicit operator Coordinates((uint X, uint Y) tuple) => new (tuple.X, tuple.Y);
    
    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static bool operator ==(Coordinates a, Coordinates b)
    {
        return a.X == b.X && a.Y == b.Y;
    }
    
    public static bool operator !=(Coordinates a, Coordinates b) => !(a == b);
    
    public override bool Equals(object? obj) => obj is Coordinates other && Equals(other);
    public bool Equals(Coordinates other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);
}