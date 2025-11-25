using GameServer.Model.Contexts;
using GameServer.Model.Entities;

namespace GameServer.Model.Systems;


public sealed class EntitySystem : BaseSystem
{
    private readonly EntityIdGenerator _generator = new();

    public GameEntity GetEntity(ulong entityId)
    {
        foreach (var game in Context.Get().Games.Values)
        {
            if (GetEntity(entityId, game) is { } ent)
                return ent;
        }
        
        throw new KeyNotFoundException($"Entity {entityId} not found");
    }

    public GameEntity? GetEntity(ulong entityId, Game game)
    {
        if (game.Entities.TryGetValue(entityId, out var entity))
            return new GameEntity( entity, game);
        else 
            return null;
    }
    
    public GameEntity CreateEntity(Game game)
    {
        var entityId = _generator.Generate();
        
        var entity = new Entity(entityId);
        game.Entities.Add(entityId, entity);
        return new GameEntity(entity, game);
    }

    public void DeleteEntity(ulong entityId)
    {
        foreach (var game in Context.Get().Games.Values
                     .Where(game => game.Entities.ContainsKey(entityId)))
        {
            DeleteEntity(entityId, game);
            return;
        }
    }

    public void DeleteEntity(ulong entityId, Game game)
    {
        if (GetEntity(entityId, game) is null)
            throw  new KeyNotFoundException($"Entity {entityId} not found in game {game.Id}");
        
        // TODO : Remove all comps 
        game.Entities.Remove(entityId);
    }

    private sealed class EntityIdGenerator()
    {
        private ulong _last = 0;
        public ulong Generate()
        {
            return _last++;
        }
    }
}