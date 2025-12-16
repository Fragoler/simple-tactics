using GameServer.Model.Action.Systems;
using GameServer.Model.Components;
using GameServer.Model.Effect;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;
using GameServer.Model.Map;

namespace GameServer.Model.Health;


public sealed class HealthSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    [Dependency] private readonly EntitySystem _entity = null!;


    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<HealthComponent, ComponentInitEvent>(OnComponentInit);
    }

    private void OnComponentInit(Entity<HealthComponent> ent, ComponentInitEvent ev)
    {
        ent.Component.CurrentHealth = ent.Component.MaxHealth;
    }

    public bool TryDealDamage(Entity entity, uint damage)
    {
        if (!_comp.TryGetComponent<HurtableComponent>(entity, out var hurtable))
            return false;

        if (!_comp.TryGetComponent<HealthComponent>(entity, out var health))
        {
            MakeDead(entity);
            return true;
        }
        
        DealDamage((entity, health), Math.Min(damage, health.MaxHealth));
        
        if (health.CurrentHealth == 0)
            MakeDead(entity);
        
        return true;
    }

    private void DealDamage(Entity<HealthComponent> target, uint damage)
    {
        Logger.LogInformation("Dealing {damage} damage to {target}", damage, target.Ent.Info.Id);
        _effect.AddEffectToQueue(new DamageEffectArgs
        {
            Amount = damage,
            Entity = target,
        });
        target.Component.CurrentHealth -= damage;
    }

    private void MakeDead(Entity entity)
    {
        Logger.LogInformation("Dead {entity}", entity);
        _effect.AddEffectToQueue(new DeathEffectArgs
        {
            Entity = entity,
        });
        
        _entity.DeleteEntity(entity);   
    }


}

