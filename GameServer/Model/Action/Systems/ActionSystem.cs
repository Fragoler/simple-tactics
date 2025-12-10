using System.Diagnostics;
using GameServer.Model.Action.Components;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly PrototypeSystem _proto = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;
    
    private readonly Dictionary<string, Entity<ActionComponent>> _actions = [];
    
    
    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<ActionComponent, ComponentInitEvent>(OnActionInit);
        _event.SubscribeLocal<ActionsContainerComponent, ComponentInitEvent>(OnContainerInit);
    }
    
    private void OnActionInit(Entity<ActionComponent> ent, ComponentInitEvent ev)
    {
        ent.Component.Effect = GetEffect(ent.Component.EffectId);
    }

    private void OnContainerInit(Entity<ActionsContainerComponent> ent, ComponentInitEvent ev)
    {
        foreach (var prototype in ent.Component.ActionPrototypes)
        {
            if (_actions.ContainsKey(prototype))
                continue;
            
            var entity = _proto.SpawnPrototype(prototype, ev.Game);
                
            Debug.Assert(_comp.TryGetComponent<ActionComponent>(entity, out var actionComp));
                
            _actions.Add(prototype, (entity, actionComp));
        }
    }

    public string[] GetAllActionIds()
    {
        return _actions.Keys.ToArray();
    }

    public Entity<ActionComponent> GetAction(string id)
    {
        return _actions[id];
    }

    public bool HasAction(string id)
    {
        return _actions.ContainsKey(id);
    }
    
}