using System.Numerics;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Map;


namespace GameServer.Model.Transform;


public sealed class TransformSystem : BaseSystem
{
    [Dependency] private readonly ComponentSystem _component = null!;
    
    public bool isValidCoords(Game game, Coordinates coords)
    {   
        var map = game.Map.Component;
        return coords.X < map.Width && coords.Y < map.Height; 
    }
    
    public void MoveEntity(Entity entity, Coordinates coords)
    {
        var xform = _component.EnsureComponent<TransformComponent>(entity);
        xform.Coords.Set(coords);
    }
    
    
}