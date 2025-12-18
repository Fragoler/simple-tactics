using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


/// <summary>
/// Attack someone at target cell. Play melee attack effect
/// </summary>
[YamlType("Melee")]
public sealed class MeleeEffect : ICellTargetActionEffect
{
    [Dependency] private readonly TransformSystem _xform = null!;
    [Dependency] private readonly HealthSystem _health = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    /// <summary>
    /// Damage
    /// </summary>
    public uint Damage { get; set; }
    
    public void Execute(Entity<TransformComponent> executor, Coordinates to)
    {
        var targets = _xform.GetEntitiesInArea(executor.Ent.Game, coords => coords == to).ToArray();

        if (targets.Length == 0)
        {
            _effect.AddEffectToQueue(new MeleeEffectArgs
            {
                Game = executor.Ent.Game,
                Entity = executor,
                From = executor.Component.Coords,
                To = to,
            });
            return;
        }
        
        var target = targets.First();

        _effect.AddEffectToQueue(new MeleeEffectArgs
        {
            Game = executor.Ent.Game,
            Entity = executor,
            From = executor.Component.Coords,
            To = to,
            Target = target,
        });
        
        _health.TryDealDamage(target, Damage);
    }
}