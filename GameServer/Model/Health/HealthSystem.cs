using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;

namespace GameServer.Model.Health;


public sealed class HealthSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;


    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<HealthComponent, ComponentInitEvent>(OnComponentInit);
    }

    private void OnComponentInit(Entity<HealthComponent> ent, ComponentInitEvent ev)
    {
        ent.Component.CurrentHealth = ent.Component.MaxHealth;
    }
}

