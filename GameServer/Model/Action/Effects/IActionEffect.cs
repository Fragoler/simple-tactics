using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


/// <summary>
/// General interface for all action's effects
/// </summary>
public interface IActionEffect
{
}

/// <summary>
/// For Action without target
/// </summary>
public interface INoneTargetActionEffect : IActionEffect
{
    void Execute(Entity<TransformComponent> executor);
}


/// <summary>
/// For action with one-cell target
/// </summary>
public interface ICellTargetActionEffect : IActionEffect
{
    void Execute(Entity<TransformComponent> executor, Coordinates to);
}