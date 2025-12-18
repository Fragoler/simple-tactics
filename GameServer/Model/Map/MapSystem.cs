using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Prototype;
using GameServer.Model.Transform;

namespace GameServer.Model.Map;


/// <summary>
/// A system to work with game map
/// </summary>
public sealed partial class MapSystem : BaseSystem
{
    [Dependency] private EventBusSystem _event = null!;
    [Dependency] private ComponentSystem _comp = null!;
    [Dependency] private PrototypeSystem _proto = null!;

    private const string MapName = "test-map";

    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<MapComponent, ComponentInitEvent>(ComponentInit);
    }


    public void LoadMapForGame(Game game)
    {
        game.Map = LoadMap(game, MapName);
    }
    
    private Entity<MapComponent> LoadMap(Game game, string mapPrototypeId)
    {
        if (!_proto.HasPrototype(mapPrototypeId))
            throw new InvalidOperationException($"Map prototype '{mapPrototypeId}' not found");

        var ent = _proto.SpawnPrototype(mapPrototypeId, game);
        
        if (!_comp.TryGetComponent<MapComponent>(ent, out var map))
            throw new InvalidOperationException($"Spawned map entity doesn't have MapComponent");

        Logger.LogInformation("Loaded map '{MapName}' ({MapId}) for game {GameToken}", 
            map.MapName, mapPrototypeId, game.Token);
        
        
        _event.RaiseGlobal(new MapLoadedEvent
        {
            Game = game,
            Map = (ent, map)
        });
        
        
        return (ent, map);
    }

    public uint GetTerrain(Game game, Coordinates cell)
    {
        var map = game.Map;
        return map.Component.Terrain!.Get(cell.X, cell.Y);
    }
}
