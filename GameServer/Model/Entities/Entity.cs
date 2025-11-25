using GameServer.Model.Components;

namespace GameServer.Model.Entities;


public sealed class Entity(ulong id)
{
    public readonly ulong Id = id;
    
    public readonly Dictionary<Type, Component> Components = [];
}

public struct GameEntity(Entity entity, Game game)
{
    public readonly Entity Entity = entity;
    public readonly Game Game = game;
}