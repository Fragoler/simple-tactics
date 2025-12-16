using GameServer.Model.Action.Components;
using GameServer.Model.Entities;
using GameServer.Model.IoC;
using GameServer.Model.Players.Components;
using GameServer.Model.Players.Events;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem
{
    [Dependency] private readonly TransformSystem _transform = null!;


    public bool TrySchedulePlayerActions(Entity<PlayerComponent> player,
        (Entity entity, string actionId, Coordinates? target)[] actions)
    {
        if (actions.Any(a => a.entity.Game != player.Ent.Game))
        {
            Logger.LogWarning("Cannot schedule player actions - client receive invalid game: {actions}", actions);
            return false;
        }
            
        
        foreach (var action in actions)
            TryScheduleAction(action);
        
        
        _event.RaiseGlobal(new PlayerActionsScheduledEvent
        {
            Game = player.Ent.Game,
            Player = player,
        });
        
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
            Logger.LogWarning("Action doesn't exist: {actionId}", actionId);
            return false;
        }
        
        if (!ValidateActionForEntity(entity, actionId))
        {
            Logger.LogWarning("Entity cannot use this action: {actionId}", actionId);
            return false;
        }

        if (target is { } coords &&
            !_transform.IsValidCoords(entity.Game, coords))
        {
            Logger.LogWarning("Action has invalid target: {target}", coords);
            return false;
        }


        if (!ValidateTarget(entity, actionId, target))
        {
            Logger.LogWarning("Target {target} is not valid for action {aciton}", target, actionId);
            return false;
        }
            
        
        return true;
    }
    
}