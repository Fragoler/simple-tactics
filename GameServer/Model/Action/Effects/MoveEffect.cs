using GameServer.Model.Action.Events;
using GameServer.Model.Action.Systems;
using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Model.Prototype;
using GameServer.Model.Terrains;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


[YamlType("Move")]
public sealed class MoveEffect : ICellTargetActionEffect, ILoggerUser
{
    [Dependency] private readonly TransformSystem _xform  = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    public ILogger Logger { get; set; } = null!;
    
    
    public int Range { get; set; } = 1;
    
    public void Execute(Entity<TransformComponent> executor, Coordinates to)
    {
        var from = _xform.GetCoords(executor);

        if (!_xform.TryMoveEntity(executor, to))
            return;
        
        _effect.AddEffectToQueue(new MoveEffectArgs
        {
            Game = executor.Ent.Game,
            Entity = executor,
            From = from,
            To = to,
            Duration = 500
        });
    }
}