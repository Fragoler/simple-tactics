using GameServer.Model.Action.Systems;
using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Phases.Events;

namespace GameServer.Model.Phases;


/// <summary>
/// Used to initiate actions' execution
/// </summary>
public sealed class PhasesSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly ActionSystem _action = null!;

    private readonly Dictionary<Game, bool> _calculating = new();

    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeGlobal<CanSchedulePlayerActionsEvent>(OnBeforeScheduling);
    }

    private void OnBeforeScheduling(CanSchedulePlayerActionsEvent ev)
    {
        if (_calculating.TryGetValue(ev.Game, out var value) &&
            value)
            ev.Cancel();
    }

    public void StartCalculating(Game game)
    {
        _calculating[game] = true;

        _action.StartExecuting(game);
        
        _calculating[game] = false;
    }
}