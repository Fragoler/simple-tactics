using GameServer.Model.Contexts;
using GameServer.Model.IoC;
using GameServer.Model.Components;
using GameServer.Model.Entities;

namespace GameServer.Model.Systems;


public class ComponentSystem : BaseSystem
{
    [Dependency] private EntitySystem _entity = null!;
    
    
    public void EnsureComponent<T>(GameEntity ent) where T : Component
    {
        if (!HasComponent<T>(ent))
            AddComponent<T>(ent);
    }
    
    public void AddComponent<T>(GameEntity ent) where T : Component
    {
        var comp = Activator.CreateInstance<T>();
        
        if (!ent.Game.Components.TryGetValue(typeof(T), out var compList))
            ent.Game.Components.Add(typeof(T), compList = []);
        
        compList.Add(comp);
        ent.Entity.Components.Add(typeof(T), comp);
    }

    public void RemoveComponent<T>(GameEntity ent) where T : Component
    {
        if (ent.Game.Components.TryGetValue(typeof(T), out var comps))
            comps.Remove(ent.Entity.Components[typeof(T)]);

        ent.Entity.Components.Remove(typeof(T));
    }

    public bool HasComponent<T>(GameEntity ent) where T : Component
    {
        return ent.Entity.Components.ContainsKey(typeof(T));
    }
}