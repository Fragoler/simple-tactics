using GameServer.Model.Components;

namespace GameServer.Model.Sprite;


/// <summary>
/// Define client sprite for unit
/// </summary>
public sealed class SpriteComponent : Component
{
    public SpriteType Type { get; set; } 
}

public enum SpriteType {
    Circle,
    Square,
    Triangle
}