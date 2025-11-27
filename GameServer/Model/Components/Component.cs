using GameServer.Model.Entities;

namespace GameServer.Model.Components;

public abstract class Component
{
    public Entity Owner { get; set; }
}
