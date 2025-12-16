using GameServer.Model.Components;

namespace GameServer.Model.Sprite;


public sealed class SpriteComponent : Component
{
    public SpriteType Type { get; set; } 
}

public enum SpriteType {
    Circle,
    Square,
    Triangle
}