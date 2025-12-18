using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


/// <summary>
/// Summon explosion
/// </summary>
[YamlType("Explosion")]
public sealed class ExplosionEffect : INoneTargetActionEffect
{
    [Dependency] private readonly TransformSystem _xform = null!;
    [Dependency] private readonly HealthSystem _health = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    
    /// <summary>
    /// Explosion range. The distance is calculated directly
    /// </summary>
    public double Range { get; set; } = 2;
    
    /// <summary>
    /// Damage 
    /// </summary>
    public uint Damage { get; set; } = 50;
    
    
    public void Execute(Entity<TransformComponent> executor)
    {
        var center = executor.Component.Coords;
        
        var targets = _xform.GetEntitiesInArea(executor.Ent.Game, 
            coords => IsInRange(coords, center, Range));

        _effect.AddEffectToQueue(new ExplosionEffectArgs
        {
            Game = executor.Ent.Game,
            Entity = executor,
            Center = center,
            Radius = Range
        });
        
        foreach (var target in targets)
            _health.TryDealDamage(target.Ent, Damage);
    }
    
    private static bool IsInRange(Coordinates coords, Coordinates center, double range)
    {
        var dx = (int)coords.X - (int)center.X;
        var dy = (int)coords.Y - (int)center.Y;
        return dx * dx + dy * dy <= range * range;
    }
}