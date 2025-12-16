using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;

namespace GameServer.Model.Effect;


public sealed class EffectSystem : BaseSystem
{
    private Dictionary<Game, List<EffectArgs>> _effectQueue = [];
    

    public void AddEffectToQueue(EffectArgs args)
    {
        if (!_effectQueue.ContainsKey(args.Entity.Game))
            _effectQueue[args.Entity.Game] = [];
        
        _effectQueue[args.Entity.Game].Add(args);
    }

    public EffectArgs[] RetrieveAllEffects(Game game)
    {
        var effects =  _effectQueue[game].ToArray();
        _effectQueue.Remove(game);
        return effects;
    }
    
    
    public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs e) 
        where TEventArgs : EventArgs;
    
    private async Task RaiseAsync<T>(AsyncEventHandler<T>? ev, T args) 
        where T : EventArgs
    {
        if (ev == null)
            return;
        
        var handlers = ev.GetInvocationList();

        var tasks = handlers
            .Cast<AsyncEventHandler<T>>()
            .Select(handler => handler(this, args));
        
        await Task.WhenAll(tasks);
    }
}