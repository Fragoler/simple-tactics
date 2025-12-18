using System.Diagnostics;
using GameServer.Model.Components;
using GameServer.Model.Games;

namespace GameServer.Model.Entities;


/// <summary>
/// Hold entity components, entity's ID and live status
/// All entity infos are saved in EntitySystem
/// </summary>
/// <param name="id">Entity unique for this game id</param>
public class EntityInfo(ulong id)
{
    public readonly ulong Id = id;
    public readonly Dictionary<Type, Component> Components = [];
    public bool Valid = true;
}

/// <summary>
/// Game entity. Can be used to operate easier with entity 
/// </summary>
/// <param name="Info">Entity info</param>
/// <param name="Game">Entity's game</param>
public readonly record struct Entity(EntityInfo Info, Game Game)
{
    public readonly EntityInfo Info = Info;
    public readonly Game Game = Game;
}


/// <summary>
/// Struct which is held game entity and its component
/// </summary>
/// <typeparam name="TComp">Entity's component type</typeparam>
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

/// <summary>
/// Struct which is held game entity and its components
/// </summary>
/// <typeparam name="TComp1">Entity's component type 1</typeparam>
/// <typeparam name="TComp2">Entity's component type 2</typeparam>
public readonly record struct Entity<TComp1, TComp2>
    where TComp1 : Component
    where TComp2 : Component
{
    public readonly Entity Ent;
    public readonly TComp1 Component1;
    public readonly TComp2 Component2;
    
    
    public static implicit operator Entity(Entity<TComp1, TComp2> entity)
    {
        return entity.Ent;
    }

    public void Deconstruct(out Entity entityId, out TComp1 comp1, out TComp2 comp2)
    {
        entityId = Ent;
        comp1 = Component1;
        comp2 = Component2;
    }

    public Entity(Entity ent, TComp1 comp1, TComp2 comp2)
    {
        Ent = ent;
        Component1 = comp1;
        Component2 = comp2;
    }
    
    public static implicit operator Entity<TComp1, TComp2>((Entity ent, TComp1 comp1, TComp2 comp2) tupl)
    {
        Debug.Assert(tupl.comp1.Owner == tupl.ent);
        Debug.Assert(tupl.comp2.Owner == tupl.ent);
        
        return new Entity<TComp1,TComp2>(tupl.ent, tupl.comp1, tupl.comp2);
    }
}