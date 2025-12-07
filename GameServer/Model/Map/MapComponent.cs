using Matrix.Core;
using GameServer.Model.Components;
using GameServer.Model.Terrains;
using GameServer.Model.Transform;

namespace GameServer.Model.Map;


public sealed class MapComponent : Component
{
    public string MapName = "Test Map";
    public uint Width  = 9;
    public uint Height = 7;

    public List<TerrainPrototype> TerrainPrototypes = [];
    public Matrix<uint>? Terrain = null;
}

public struct TerrainPrototype
{
    public Coordinates[] CoordsList = [];
    public uint Type = 0;

    public TerrainPrototype()
    {
    }
}



