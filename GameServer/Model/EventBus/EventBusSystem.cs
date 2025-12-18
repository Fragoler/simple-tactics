using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.IoC;

namespace GameServer.Model.EventBus;



/// <summary>
/// A system that provides instruments to raise actions and subscribe on it
/// </summary>
public sealed class EventBusSystem : BaseSystem
{
    [Dependency] private readonly ComponentSystem _comp = null!;

    private Dictionary<Type, List<Action<IBaseEvent>>> _publicSubs = [];
    private Dictionary<Type, 
        List<Action<Entity, IBaseEvent>>> _subs = [];                                          // Simple subs
    private Dictionary<Type, 
        List<(Type compType, Action<Entity, Component, IBaseEvent> callback)>> _compSubs = []; // Only entity with comp subs


    public void SubscribeGlobal<TEvent>(Action<TEvent> handler)
        where TEvent : IBaseEvent
    {
        var  evType = typeof(TEvent);
        if (!_publicSubs.ContainsKey(evType))
            _publicSubs[evType] = [];
        
        _publicSubs[evType].Add(Wrapper);
        return;

        void Wrapper(IBaseEvent ev)
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

        void Wrapper(Entity ent, IBaseEvent ev)
        {
            if (ev is TEvent tev)
                handler(ent, tev);
        }
    }
    
    public void SubscribeLocal<TComp, TEvent>(Action<Entity<TComp>, TEvent> handler)
        where TComp : Component
        where TEvent : IBaseEvent
    {
        var evType = typeof(TEvent);
        if (!_compSubs.ContainsKey(evType))
            _compSubs[evType] = [];
        
        _compSubs[evType].Add((typeof(TComp), Wrapper));
        return;

        void Wrapper(Entity entity, Component comp, IBaseEvent ev)
        {
            if (ev is TEvent tEv &&
                comp is TComp tComp)
                handler((entity, tComp), tEv);
        }
    }

    public void RaiseCompLifeCircle(Entity entity, IComponentLifecycleEvent ev)
    {
        if (!_compSubs.TryGetValue(ev.GetType(), out var compSubs))
            return;
        
        foreach (var sub in compSubs
                     .Where(p => ev.CompType == p.compType)
                     .Where(p => _comp.HasComponent(entity, p.compType)))
            sub.callback(entity, _comp.GetComponentOrDefault(entity, sub.compType)!, ev);
    }
    

    public void RaiseGlobal(IBaseEvent ev)
    {
        if (!_publicSubs.TryGetValue(ev.GetType(), out var subs))
            return;
        
        foreach (var sub in subs)
            sub(ev);
    }

    public void RaiseLocal(Entity entity, IBaseEvent ev)
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
