using GameServer.Model.Action.Components;
using GameServer.Model.Action.Effects;
using GameServer.Model.Entities;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem
{
    private bool ValidateTarget(Entity entity, string actionId, Coordinates? target)
    {
        if (!_comp.TryGetComponent<TransformComponent>(entity, out var xform))
            return false;

        var action = GetAction(actionId);
        var pattern = action.Component.TargetFilter.Pattern;
        var effect = action.Component.Effect;

        if (target is null)
            return effect is INoneTargetActionEffect;
        
        return pattern.Validate(xform.Coords, target.Value);
    }
    
    private bool ValidateActionForEntity(Entity entity, string actionId)
    {
        return _comp.TryGetComponent<ActionsContainerComponent>(entity, out var container)
            ? container.ActionPrototypes.Contains(actionId)
            : throw new ArgumentException("Cannot find actions container for entity");
    }
}