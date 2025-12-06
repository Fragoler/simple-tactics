using System.Diagnostics;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Model.Players;
using GameServer.Model.Prototype;

namespace GameServer.Model;


public sealed class TestSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly PlayersSystem _player = null!;
    [Dependency] private readonly PrototypeSystem _proto = null!;
    
    public override void Initialize()
    {
        base.Initialize();

        _event.SubscribeGlobal<AfterMapLoadedEvent>(SummonTestEntities);
    }

    private void SummonTestEntities(AfterMapLoadedEvent ev)
    {
        var unit1 = _proto.SpawnPrototype("test-unit", ev.Game);
        var unit2 = _proto.SpawnPrototype("test-unit", ev.Game);
        
        Debug.Assert(ev.Game.Players.Count >= 2);
        
        _player.AssignUnitToPlayer(ev.Game.Players[0], unit1);
        _player.AssignUnitToPlayer(ev.Game.Players[1], unit2);
    }
}