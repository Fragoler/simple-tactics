using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;

namespace GameServer.Model.Map;


public sealed class MapSystem : BaseSystem
{
    [Dependency] private EventBusSystem _event = null!;
    [Dependency] private ComponentSystem _comp = null!;
    [Dependency] private PrototypeSystem _proto = null!;

    private const string MapName = "test-map";
    
    
    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeGlobal<GameCreatedEvent>(LoadMapForGame);
    }

    
    private void LoadMapForGame(GameCreatedEvent ev)
    {
        ev.Game.Map = LoadMap(ev.Game, MapName);
    }
    
    private Entity<MapComponent> LoadMap(Game game, string mapPrototypeId)
    {
        if (!_proto.HasPrototype(mapPrototypeId))
            throw new InvalidOperationException($"Map prototype '{mapPrototypeId}' not found");

        var mapEntity = _proto.SpawnPrototype(mapPrototypeId, game);
        
        if (!_comp.TryGetComponent<MapComponent>(mapEntity, out var mapComp))
            throw new InvalidOperationException($"Spawned map entity doesn't have MapComponent");

        Logger.LogInformation("Loaded map '{MapName}' ({MapId}) for game {GameToken}", 
            mapComp.MapName, mapPrototypeId, game.Token);

        return (mapEntity, mapComp);
    }
}
