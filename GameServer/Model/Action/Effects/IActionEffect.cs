using GameServer.Model.Games;
using GameServer.Model.Transform;

namespace GameServer.Model.Action.Effects;


public interface IActionEffect
{
}

public interface INoneTargetActionEffect : IActionEffect
{
    void Execute();
}


public interface ICellTargetActionEffect : IActionEffect
{
    void Execute(Coordinates coords);
}