using GameServer.Model.Action.Components;
using GameServer.Model.Entities;
using GameServer.Model.IoC;
using GameServer.Model.Players.Components;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem
{
    [Dependency] private readonly TransformSystem _transform = null!;


    public bool TrySchedulePlayerActions(Entity<PlayerComponent> player,
        (Entity entity, string actionId, Coordinates? target)[] actions)
    {
        if (actions.Any(a => a.entity.Game != player.Ent.Game))
            return false;
        
        if (actions.Any(a => !CanScheduleAction(a)))
            return false;
        
        foreach (var action in actions)
            TryScheduleAction(action);
        
        return true;
    }

    public bool TryScheduleAction((Entity entity, string actionId, Coordinates? target) action)
    {
        return TryScheduleAction(action.entity, action.actionId, action.target);
    }
    
    public bool TryScheduleAction(Entity entity, string actionId, Coordinates? target = null)
    {
        if (!CanScheduleAction(entity, actionId, target))
            return false;
        
        var scheduled = _comp.EnsureComponent<ScheduledActionComponent>(entity);
        
        scheduled.Target = target;
        scheduled.ActionId = actionId;

        return true;
    }


    public bool CanScheduleAction((Entity entity, string actionId, Coordinates? target) action)
    {
        return CanScheduleAction(action.entity, action.actionId, action.target);
    }
    
    public bool CanScheduleAction(Entity entity, string actionId, Coordinates? target = null)
    {
        if (!HasAction(actionId))
        {
            Logger.LogError("Action doesn't exist: {actionId}", actionId);
            return false;
        }
        
        if (!ValidateActionForEntity(entity, actionId))
        {
            Logger.LogError("Entity cannot use this action: {actionId}", actionId);
            return false;
        }

        if (target is { } coords &&
            !_transform.isValidCoords(entity.Game, coords))
            return false;
        
        return true;
    }

    private bool ValidateActionForEntity(Entity entity, string actionId)
    {
        return _comp.TryGetComponent<ActionsContainerComponent>(entity, out var container)
            ? container.ActionPrototypes.Contains(actionId)
            : throw new ArgumentException("Cannot find actions container for entity");
    }
    
}