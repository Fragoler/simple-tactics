using System.Numerics;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Map;


namespace GameServer.Model.Transform;


public sealed class TransformSystem : BaseSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;
    
    public bool isValidCoords(Game game, Coordinates coords)
    {   
        var map = game.Map.Component;
        return coords.X < map.Width && coords.Y < map.Height; 
    }
    
    public void MoveEntity(Entity entity, Coordinates coords)
    {
        var xform = _comp.EnsureComponent<TransformComponent>(entity);
        xform.Coords = coords;
    }


    public Coordinates GetCoords(Entity entity)
    {
        if (!_comp.TryGetComponent<TransformComponent>(entity, out var xform))
            throw new ArgumentException("Cannot find xform component to get coords");

        return GetCoords((entity, xform));
    }
    
    public Coordinates GetCoords(Entity<TransformComponent> entity)
    {
        return entity.Component.Coords;
    }

    public void SetCoords(Entity entity, Coordinates coords)
    {
        if (!_comp.TryGetComponent<TransformComponent>(entity, out var xform))
            throw new ArgumentException("Cannot find xform component to set coords");

        SetCoords((entity, xform), coords);
    }

    
    public void SetCoords(Entity<TransformComponent> entity, Coordinates coords)
    {
        entity.Component.Coords = coords;
    }

    public IEnumerable<Entity<TransformComponent>> GetEntitiesInArea(Game game, Func<Coordinates, bool> areaFunc)
    {
        var queue =_entity.GetAllEntity(game);
        var finded = new List<Entity<TransformComponent>>();
        foreach (var ent in queue)
        {
            if (!_comp.TryGetComponent<TransformComponent>(ent, out var xform))
                continue;

            if (areaFunc(xform.Coords))
                finded.Add((ent, xform));
        }
        return finded;
    }
    
    
}