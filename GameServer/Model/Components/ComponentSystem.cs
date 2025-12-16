using System.Collections;
using System.Diagnostics.CodeAnalysis;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;

namespace GameServer.Model.Components;


public class ComponentSystem : BaseSystem
{
    [Dependency] private EventBusSystem _event = null!;

    public override void Initialize()
    {
        base.Initialize();

        _event.SubscribeLocal<EntityRemoveEvent>(RemoveAllComponents);
    }

    private void RemoveAllComponents(Entity ent, EntityRemoveEvent ev)
    {
        var componentTypes = ent.Info.Components.Keys.ToList();
        foreach (var compType in componentTypes)
            RemoveComponent(ent, compType);
    }

    public T? GetComponentOrDefault<T>(Entity ent) where T : Component
    {
        if (ent.Info.Components.TryGetValue(typeof(T), out var comp))
            return (T)comp;
        return null;
    }

    public Component? GetComponentOrDefault(Entity ent, Type type)
    {
        return ent.Info.Components.GetValueOrDefault(type);
    }

    public bool TryGetComponent<T>(Entity ent, [NotNullWhen(true)] out T? comp) where T : Component
    {
        comp = GetComponentOrDefault<T>(ent);
        return comp != null;
    }

    public T EnsureComponent<T>(Entity ent) where T : Component, new()
    {
        if (!TryGetComponent<T>(ent, out var comp))
            comp = AddComponent<T>(ent);
        return comp;
    }
    
    public void RemoveComponent<T>(Entity ent) where T : Component
    {
        RemoveComponent(ent, typeof(T));
    }

    public void RemoveComponent(Entity ent, Type compType)
    {
        ent.Info.Components.Remove(compType);
    }

    public bool HasComponent<T>(Entity ent) where T : Component
    {
        return HasComponent(ent, typeof(T));
    }

    public bool HasComponent(Entity ent, Type compType)
    {
        return ent.Info.Components.ContainsKey(compType);
    }
    

    public Component AddComponent(Entity ent, Type compType, Dictionary<string, object>? data = null)
    {
        var comp = (Component)Activator.CreateInstance(compType)!;
        if (data != null)
            ApplyComponentData(comp, data);

        AddComponent(ent, comp);
        return comp;
    }

    private void ApplyComponentData(Component component, Dictionary<string, object> data)
    {
        var componentType = component.GetType();
        
        foreach (var kvp in data)
        {
            var property = componentType.GetProperty(kvp.Key);
            if (property != null && property.CanWrite)
            {
                property.SetValue(component, kvp.Value);
                continue;
            }
            
            var field = componentType.GetField(kvp.Key);
            if (field != null)
            {
                field.SetValue(component, kvp.Value);
            }
        }
    }
    public T AddComponent<T>(Entity ent) where T : Component, new()
    {
        return (T)AddComponent(ent, new T());
    }
    
    public Component AddComponent(Entity ent, Component comp)
    {
        comp.Owner = ent;
        ent.Info.Components.Add(comp.GetType(), comp);
        
        _event.RaiseLocal(ent, new ComponentInitEvent
        {
            CompType = comp.GetType(),
            Game = ent.Game
        });
        
        return comp;
    }
}