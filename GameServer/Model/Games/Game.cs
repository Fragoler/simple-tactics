using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Map;
using GameServer.Model.Players;
using GameServer.Model.Players.Components;

namespace GameServer.Model.Games;



/// <summary>
/// A class to save info about current game
/// </summary>
/// <param name="token">Unique game's token</param>
public class Game(string token)
{
    public readonly string Token = token;
    
    public readonly List<Entity<PlayerComponent>> Players = [];
    public Entity<MapComponent> Map;
    
    // Game entities
    public readonly Dictionary<ulong, EntityInfo> Entities = new();
}