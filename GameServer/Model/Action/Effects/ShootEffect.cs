using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Model.Prototype;
using GameServer.Model.Terrains;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


[YamlType("Shoot")]
public sealed class ShootEffect : ICellTargetActionEffect
{
    [Dependency] private readonly TransformSystem _xform = null!;
    [Dependency] private readonly HealthSystem _health = null!;
    [Dependency] private readonly MapSystem _map = null!;
    [Dependency] private readonly TerrainSystem _terrain = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    public uint Damage { get; set; }


    public void Execute(Entity<TransformComponent> executor, Coordinates to)
    {
        var targets = _xform.GetEntitiesInArea(executor.Ent.Game,
            coords => InSquare(coords, executor.Component.Coords, to)).ToArray();

        var line = GetBresenhamLine(executor.Component.Coords, to);

        foreach (var point in line.Skip(1))
        {
            if (!_terrain.IsPassableForBullet(_map.GetTerrain(executor.Ent.Game, point)))
            {
                _effect.AddEffectToQueue(new ShootEffectArgs
                {
                    Game = executor.Ent.Game,
                    Entity = executor,
                    From = executor.Component.Coords,
                    To = point,
                });
                return;
            }

            var hit = targets.FirstOrDefault(t =>
                t.Component.Coords.X == point.X &&
                t.Component.Coords.Y == point.Y);

            if (hit.Equals(default)) continue;
            
            _effect.AddEffectToQueue(new ShootEffectArgs
            {
                Game = executor.Ent.Game,
                Entity = executor,
                From = executor.Component.Coords,
                To = point,
                Target = hit,
            });

            _health.TryDealDamage(hit, Damage);
            return;
        }
        
        
        _effect.AddEffectToQueue(new ShootEffectArgs
        {
            Game = executor.Ent.Game,
            Entity = executor,
            From = executor.Component.Coords,
            To = to,
        });
    }


    private static bool InSquare(Coordinates coords, Coordinates vert1, Coordinates vert2)
    {
        if (vert1.X <= vert2.X)
        {
            if (coords.X < vert1.X || coords.X > vert2.X)
                return false;
        }
        else
        {
            if (coords.X < vert2.X || coords.X > vert1.X)
                return false;
        }
        
        if (vert1.Y <= vert2.Y)
        {
            if (coords.Y < vert1.Y || coords.Y > vert2.Y)
                return false;
        }
        else
        {
            if (coords.Y < vert2.Y || coords.Y > vert1.Y)
                return false;
        }
        
        return true;
    }
    
       
    private static List<Coordinates> GetBresenhamLine(Coordinates from, Coordinates to)
    {
        var points = new List<Coordinates>();
        
        var x0 = (int)from.X;
        var y0 = (int)from.Y;
        var x1 = (int)to.X;
        var y1 = (int)to.Y;
    
        var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            (x0, y0) = (y0, x0);
            (x1, y1) = (y1, x1);
        }

        var swapped = false;
        if (x0 > x1)
        {
            (x0, x1) = (x1, x0);
            (y0, y1) = (y1, y0);
            swapped = true;
        }
    
        var dx = x1 - x0;
        var dy = Math.Abs(y1 - y0);
        var error = dx / 2;
        var ystep = y0 < y1 ? 1 : -1;
        var y = y0;
    
        for (var x = x0; x <= x1; x++)
        {
            var coord = steep 
                ? new Coordinates((uint)y, (uint)x) 
                : new Coordinates((uint)x, (uint)y);
            points.Add(coord);
        
            error -= dy;
            
            if (error >= 0) continue;
            
            y += ystep;
            error += dx;
        }
        
        if (swapped)
        {
            points.Reverse();
        }
    
        return points;
    }

}