using GameServer.Model.Entities;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


[YamlType("Melee")]
public sealed class MeleeEffect : ICellTargetActionEffect
{
    [Dependency] private readonly TransformSystem _xform = null!;
    [Dependency] private readonly HealthSystem _health = null!;
    
    
    public uint Damage { get; set; }
    
    public void Execute(Entity<TransformComponent> executor, Coordinates to)
    {
        var targets = _xform.GetEntitiesInArea(executor.Ent.Game, coords => coords == to).ToArray();

        if (targets.Length == 0)
            return;

        foreach (var target in targets)
            _health.TryDealDamage(target, Damage);
    }
}