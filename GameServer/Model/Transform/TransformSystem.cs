using System.Numerics;
using GameServer.Model.Action.Events;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Map;


namespace GameServer.Model.Transform;


/// <summary>
/// A system that provides operations to entity moving
/// </summary>
public sealed class TransformSystem : BaseSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;
    [Dependency] private readonly EventBusSystem _event = null!;


    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<AttemptMoveEvent>(OnMoveAttempt);
    }

    private void OnMoveAttempt(Entity ent, AttemptMoveEvent ev)
    {
        var queue =_entity.GetAllEntity(ev.Game);

        foreach (var entity in queue)
        {
            if (!_comp.TryGetComponent<TransformComponent>(entity, out var xform))
                continue;

            if (xform.Coords != ev.To) 
                continue;
            
            ev.Cancel();
            return;
        }
        
    }

    public bool IsValidCoords(Game game, Coordinates coords)
    {   
        var map = game.Map.Component;
        return coords.X < map.Width && coords.Y < map.Height; 
    }
    
    public bool TryMoveEntity(Entity<TransformComponent> entity, Coordinates coords)
    {
        var ev = new AttemptMoveEvent
        {
            Game = entity.Ent.Game,
            From = entity.Component.Coords,
            To = coords,
        };
        _event.RaiseLocal(entity, ev);
        if (ev.Cancelled)
            return false;
        
        SetCoords(entity, coords);
        return true;
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

    private void SetCoords(Entity entity, Coordinates coords)
    {
        if (!_comp.TryGetComponent<TransformComponent>(entity, out var xform))
            throw new ArgumentException("Cannot find xform component to set coords");

        SetCoords((entity, xform), coords);
    }
    
    private void SetCoords(Entity<TransformComponent> entity, Coordinates coords)
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