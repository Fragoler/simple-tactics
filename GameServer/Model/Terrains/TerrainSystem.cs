using GameServer.Model.Action.Events;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Model.Transform;

namespace GameServer.Model.Terrains;



/// <summary>
/// A system that works with terrains
/// </summary>
public sealed class TerrainSystem : BaseSystem
{
    [Dependency] private readonly EventBusSystem _event = null!;
    [Dependency] private readonly MapSystem _map = null!;
    
    private ITerrain[] _terrains = [
        new Ground(),
        new Wall(),
        new Ground()
    ];

    public override void Initialize()
    {
        base.Initialize();
        
        _event.SubscribeLocal<AttemptMoveEvent>(OnMoveEvent);
    }


    private void OnMoveEvent(Entity entity, AttemptMoveEvent ev)
    {
        if (!GetTerrainById(_map.GetTerrain(ev.Game, ev.To)).PassableForUnit)
            ev.Cancel();
    }


    public ITerrain GetTerrainById(uint terrainId)
    {
        return _terrains.Length >  terrainId 
            ? _terrains[terrainId] 
            : throw new ArgumentOutOfRangeException($" {nameof(terrainId)}, {terrainId}");
    }

    public uint GetIdByTerrain(ITerrain terrain)
    {
        return Convert.ToUInt32(Array.FindIndex(_terrains, elem => elem.GetType() == terrain.GetType()));
    }

    public bool IsPassableForBullet(uint terrainId)
    {
        return GetTerrainById(terrainId).PassableForBullet;
    }
    
    public bool IsPassableForUnit(uint terrainId)
    {
        return GetTerrainById(terrainId).PassableForUnit;
    }
}