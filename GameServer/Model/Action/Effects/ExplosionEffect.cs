using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


[YamlType("Explosion")]
public sealed class ExplosionEffect : INoneTargetActionEffect
{
    [Dependency] private readonly TransformSystem _xform = null!;
    [Dependency] private readonly HealthSystem _health = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    public uint Radius { get; set; } = 2;
    public uint Damage { get; set; } = 50;
    
    
    public void Execute(Entity<TransformComponent> executor)
    {
        var center = executor.Component.Coords;
        
        var targets = _xform.GetEntitiesInArea(executor.Ent.Game, 
            coords => IsInRadius(coords, center, Radius));

        _effect.AddEffectToQueue(new ExplosionEffectArgs
        {
            Game = executor.Ent.Game,
            Entity = executor,
            Center = center,
            Radius = Radius
        });
        
        foreach (var target in targets)
            _health.TryDealDamage(target.Ent, Damage);
    }
    
    private static bool IsInRadius(Coordinates coords, Coordinates center, uint radius)
    {
        var dx = (int)coords.X - (int)center.X;
        var dy = (int)coords.Y - (int)center.Y;
        return dx * dx + dy * dy <= radius * radius;
    }
}