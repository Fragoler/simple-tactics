using GameServer.Model.Entities;

namespace GameServer.Model.Components;


/// <summary>
/// The basic for all entities' properies
/// Can be uniquely added to all entity 
/// </summary>
public abstract class Component
{
    public Entity Owner { get; set; }
}
