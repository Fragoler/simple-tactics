using GameServer.Model.Effects;
using GameServer.Model.Entities;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


/// <summary>
/// Move entity to target cell
/// </summary>
[YamlType("Move")]
public sealed class MoveEffect : ICellTargetActionEffect, ILoggerUser
{
    [Dependency] private readonly TransformSystem _xform  = null!;
    [Dependency] private readonly EffectSystem _effect = null!;
    
    public ILogger Logger { get; set; } = null!;
    
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