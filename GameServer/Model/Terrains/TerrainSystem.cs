using GameServer.Model.IoC;

namespace GameServer.Model.Terrains;


public sealed class TerrainSystem : BaseSystem
{
    private ITerrain[] _terrains = [
        new Ground(),
        new Wall()
    ];

    
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
}