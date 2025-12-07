using System.Diagnostics;
using GameServer.Model.Components;
using GameServer.Model.Games;

namespace GameServer.Model.Entities;


public class EntityInfo(ulong id)
{
    public readonly ulong Id = id;
    
    public readonly Dictionary<Type, Component> Components = [];
}

public readonly record struct Entity(EntityInfo Info, Game Game)
{
    public readonly EntityInfo Info = Info;
    public readonly Game Game = Game;
}

public readonly record struct Entity<TComp>
    where TComp : Component
{
    public readonly Entity Ent;
    public readonly TComp Component;
    
    public static implicit operator Entity(Entity<TComp> entity)
    {
        return entity.Ent;
    }

    public void Deconstruct(out Entity entityId, out TComp component)
    {
        entityId = Ent;
        component = Component;
    }

    public Entity(Entity ent, TComp component)
    {
        Ent = ent;
        Component = component;
    }
    
    public static implicit operator Entity<TComp>((Entity ent, TComp comp) tupl)
    {
        Debug.Assert(tupl.comp.Owner == tupl.ent);
        return new Entity<TComp>(tupl.ent, tupl.comp);
    }
}
