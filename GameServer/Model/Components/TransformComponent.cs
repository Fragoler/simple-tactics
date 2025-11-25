namespace GameServer.Model.Components;

public sealed class TransformComponent : Component
{
    public Coordinates coords = new Coordinates(0, 0);
}

public sealed class Coordinates(uint x, uint y)
{
    public uint X = x;
    public uint Y = y;
}    
