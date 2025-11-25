using GameServer.Model.Entities;
using GameServer.Model.Contexts;
using GameServer.Model.IoC;

namespace GameServer.Model.Systems;


public class GamesSystem : BaseSystem
{
    [Dependency] private EntitySystem _entity = null!;
    
    
    public Game GetGameById(Guid gameId)
    {
        return Context.Get().Games.TryGetValue(gameId, out var game) ? game : 
                throw new  KeyNotFoundException($"Game with id {gameId} does not exist.");
    }

    public Game GetGameByEntId(ulong entityId)
    {
        foreach (var game in Context.Get().Games.Values)
        {
            if (_entity.GetEntity(entityId, game) == null)
                continue;

            return game;
        }
        
        throw new  KeyNotFoundException($"Game with id {entityId} does not exist.");
    }
}