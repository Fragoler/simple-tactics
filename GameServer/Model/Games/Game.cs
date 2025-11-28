using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Players;

namespace GameServer.Model.Games;


public class Game(string token)
{
    public readonly string Token = token;
    
    public List<(Entity, PlayerComponent)> Players = [];
    public Entity Field;
    
    // Game entities
    public readonly Dictionary<ulong, EntityInfo> Entities = new();
}