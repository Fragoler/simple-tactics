using GameServer.Model.Games;
using GameServer.Model.IoC;

namespace GameServer.Model.Effects;


public sealed class EffectSystem : BaseSystem
{
    private readonly Dictionary<Game, List<EffectArgs>> _effectQueue = [];
    

    public void AddEffectToQueue(EffectArgs args)
    {
        if (!_effectQueue.ContainsKey(args.Game))
            _effectQueue[args.Game] = [];
        
        _effectQueue[args.Game].Add(args);
    }

    public EffectArgs[] RetrieveAllEffects(Game game)
    {
        var effects =  _effectQueue[game].ToArray();
        _effectQueue.Remove(game);
        return effects;
    }
}