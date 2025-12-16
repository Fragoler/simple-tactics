using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


public interface IActionEffect
{
}

public interface INoneTargetActionEffect : IActionEffect
{
    void Execute(Entity<TransformComponent> executor);
}


public interface ICellTargetActionEffect : IActionEffect
{
    void Execute(Entity<TransformComponent> executor, Coordinates to);
}