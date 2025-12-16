using System.Diagnostics;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Health;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Model.Players;
using GameServer.Model.Players.Systems;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model;


public sealed class TempSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly PlayersSystem _player = null!;
    [Dependency] private readonly PrototypeSystem _proto = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;
    
    public override void Initialize()
    {
        base.Initialize();

        _event.SubscribeGlobal<MapLoadedEvent>(SummonTestEntities);
    }

    private void SummonTestEntities(MapLoadedEvent ev)
    {
        List<Entity> units1 = [], units2 = [];
        
        units1.Add(_proto.SpawnPrototype("Triangle", ev.Game));
        units1.Add(_proto.SpawnPrototype("Square", ev.Game));
        units1.Add(_proto.SpawnPrototype("Circle", ev.Game));
        
        units2.Add(_proto.SpawnPrototype("Circle", ev.Game));
        units2.Add(_proto.SpawnPrototype("Triangle", ev.Game));
        units2.Add(_proto.SpawnPrototype("Square", ev.Game));

        var neutral = _proto.SpawnPrototype("Square", ev.Game);
        
        _comp.EnsureComponent<TransformComponent>(units1[0]).Coords = (1, 1);
        _comp.EnsureComponent<TransformComponent>(units1[1]).Coords = (1, 5);
        _comp.EnsureComponent<TransformComponent>(units1[2]).Coords = (2, 3);
        
        _comp.EnsureComponent<TransformComponent>(units2[0]).Coords = (6, 1);
        _comp.EnsureComponent<TransformComponent>(units2[1]).Coords = (6, 5);
        _comp.EnsureComponent<TransformComponent>(units2[2]).Coords = (7, 3);
        
        _comp.EnsureComponent<TransformComponent>(neutral).Coords = (4, 3);
        
        Debug.Assert(ev.Game.Players.Count >= 2);
        
        _player.AssignUnitToPlayer(ev.Game.Players[0], units1[0]);
        _player.AssignUnitToPlayer(ev.Game.Players[0], units1[1]);
        _player.AssignUnitToPlayer(ev.Game.Players[0], units1[2]);
        
        _player.AssignUnitToPlayer(ev.Game.Players[1], units2[0]);
        _player.AssignUnitToPlayer(ev.Game.Players[1], units2[1]);
        _player.AssignUnitToPlayer(ev.Game.Players[1], units2[2]);
        
        Logger.LogInformation("Units Created");
    }
}