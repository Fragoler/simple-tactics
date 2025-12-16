using GameServer.Model.Action.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Transform;
using System.Linq;
using GameServer.Model.Action.Effects;
using GameServer.Model.Phases.Events;

namespace GameServer.Model.Action.Systems;


public sealed partial class ActionSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    
    public void StartExecuting(Game game)
    {
        var rnd = new Random();
        var scheduled = _entity.GetAllEntity(game)
            .Where(_comp.HasComponent<TransformComponent>)
            .Where(_comp.HasComponent<ScheduledActionComponent>)
            .Select(e => new Entity<TransformComponent, ScheduledActionComponent>(
                e, _comp.GetComponentOrDefault<TransformComponent>(e)!,
                _comp.GetComponentOrDefault<ScheduledActionComponent>(e)!))
            .OrderBy(e => GetAction(e.Component2.ActionId).Component.Pace)
            .ThenBy(e => rnd.Next())
            .ToArray();

        Logger.LogInformation("Starting executing");
        foreach (var entity in scheduled)
        {
            var effect = GetAction(entity.Component2.ActionId).Component.Effect;
            Execute((entity.Ent, entity.Component1), effect, entity.Component2.Target);
        }
        
        _event.RaiseGlobal(new EndExecutingPhaseEvent
        {
            Game = game
        });
    }

    private void Execute(Entity<TransformComponent> executor, IActionEffect effect, Coordinates? target)
    {
        Logger.LogInformation("Executing {executor} - {effect}, coords: {coords}", executor.Ent.Info.Id,
            effect.GetType().Name, target);
        
        switch (effect)
        {
            case ICellTargetActionEffect cellTarget:
                cellTarget.Execute(executor, target!.Value);
                break;
            case INoneTargetActionEffect noneTarget:
                noneTarget.Execute(executor);
                break;
            default:
                throw new NotImplementedException($"Unhandled effect type {effect.GetType().Name}");
        }
    }
}