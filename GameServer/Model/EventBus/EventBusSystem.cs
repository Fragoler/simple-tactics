using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.IoC;

namespace GameServer.Model.EventBus;


public sealed class EventBusSystem : BaseSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;

    private Dictionary<Type, List<Action<BaseEvent>>> _publicSubs = [];
    private Dictionary<Type, List<Action<Entity, BaseEvent>>> _subs = [];                                          // Simple subs
    private Dictionary<Type, List<(Type compType, Action<Entity, Component, BaseEvent> callback)>> _compSubs = []; // Only entity with comp subs


    public void SubscribeGlobal<TEvent>(Action<TEvent> handler)
        where TEvent : BaseEvent
    {
        var  evType = typeof(TEvent);
        if (!_publicSubs.ContainsKey(evType))
            _publicSubs[evType] = [];
        
        _publicSubs[evType].Add(Wrapper);
        return;

        void Wrapper(BaseEvent ev)
        {
            if (ev is TEvent tev)
                handler(tev);
        }
    }
    
    public void SubscribeLocal<TEvent>(Action<Entity, TEvent> handler)
    {
        var evType = typeof(TEvent);
        if (!_subs.ContainsKey(evType))
            _subs[evType] = [];
        
        _subs[evType].Add(Wrapper);
        return;

        void Wrapper(Entity ent, BaseEvent ev)
        {
            if (ev is TEvent tev)
                handler(ent, tev);
        }
    }
    
    public void SubscribeLocal<TComp, TEvent>(Action<Entity<TComp>, TEvent> handler)
        where TComp : Component
        where TEvent : BaseEvent
    {
        var evType = typeof(TEvent);
        if (!_compSubs.ContainsKey(evType))
            _compSubs[evType] = [];
        
        _compSubs[evType].Add((typeof(TComp), Wrapper));
        return;

        void Wrapper(Entity entity, Component comp, BaseEvent ev)
        {
            if (ev is TEvent tEv &&
                comp is TComp tComp)
                handler((entity, tComp), tEv);
        }
    }


    public void RaiseGlobal(BaseEvent ev)
    {
        if (_publicSubs.TryGetValue(ev.GetType(), out var subs))
        {
            foreach (var sub in subs)
                sub(ev);
        }
    }
    
    public void RaiseLocalForAll(BaseEvent ev)
    {
        foreach (var ent in _entity.GetAllEntity(ev.Game))
            RaiseLocal(ent, ev);
    }
    
    public void RaiseLocal(Entity entity, BaseEvent ev)
    {
        if (_subs.TryGetValue(ev.GetType(), out var subs))
        {
            foreach (var sub in subs)
                sub(entity, ev);
        }
        
        if (_compSubs.TryGetValue(ev.GetType(), out var compSubs))
        {
            foreach (var sub in compSubs.Where(p => _comp.HasComponent(entity, p.compType)))
                sub.callback(entity, _comp.GetComponentOrDefault(entity, sub.compType)!, ev);
        }
    }
}
