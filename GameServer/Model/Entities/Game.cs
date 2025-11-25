using GameServer.Model.Components;

namespace GameServer.Model.Entities;


public class Game
{
    public Guid Id;
    
    public readonly Dictionary<ulong, Entity> Entities = new();
    public readonly Dictionary<Type, HashSet<Component>> Components = new();
}