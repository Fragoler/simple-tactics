using GameServer.Model.Components;
using GameServer.Model.Games;

namespace GameServer.Model.Entities;


public class EntityInfo(ulong id)
{
    public readonly ulong Id = id;
    
    public readonly Dictionary<Type, Component> Components = [];
}

public record struct Entity(EntityInfo Info, Game Game)
{
    public readonly EntityInfo Info = Info;
    public readonly Game Game = Game;
}
