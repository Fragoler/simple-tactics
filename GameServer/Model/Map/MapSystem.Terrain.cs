using GameServer.Model.Components;
using GameServer.Model.Entities;
using Matrix.Core;

namespace GameServer.Model.Map;


public sealed partial class MapSystem
{
    private void ComponentInit(Entity<MapComponent> map, ComponentInitEvent ev)
    {
        map.Component.Terrain = new Matrix<uint>(map.Component.Width, map.Component.Height);

        for (uint x = 0; x < map.Component.Terrain.Width; x++)
        for (uint y = 0; y < map.Component.Terrain.Height; y++)
            map.Component.Terrain[x, y] = 0;
        
        foreach (var proto in map.Component.TerrainPrototypes)
            foreach (var coord in proto.CoordsList)
                map.Component.Terrain[coord.X, coord.Y] = proto.Type;

        Logger.LogInformation("Setup terrain {matrix}", map.Component.Terrain.ToString());
    }
}