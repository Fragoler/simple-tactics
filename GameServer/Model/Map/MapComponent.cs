using Matrix.Core;
using GameServer.Model.Components;
using GameServer.Model.Terrains;
using GameServer.Model.Transform;

namespace GameServer.Model.Map;


/// <summary>
/// Hold info about game map
/// </summary>
public sealed class MapComponent : Component
{
    public string MapName { get; set; } = "Test Map";
    public uint Width { get; set; } = 9;
    public uint Height { get; set; } = 7;

    public List<TerrainPrototype> TerrainPrototypes { get; set; } = [];
    public Matrix<uint>? Terrain = null;
}

public struct TerrainPrototype()
{
    public Coordinates[] CoordsList { get; set; } = [];
    public uint Type { get; set; } = 0;
}



