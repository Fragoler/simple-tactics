using GameServer.Model.Components;

namespace GameServer.Model.Map;


public sealed class MapComponent : Component
{
    public string MapName { get; set; } = "Test Map";
    public int Width { get; set; } = 9;
    public int Height { get; set; } = 7;
    // public List<string> TerrainTiles { get; set; } = new();
}