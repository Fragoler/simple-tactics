using System.Diagnostics.CodeAnalysis;
using GameServer.Model.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;

namespace GameServer.Model.Players;


public sealed class PlayersSystem : BaseSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;

    private readonly HashSet<Entity> _players = [];
    
    public void CreateAttachedPlayer(Game game, out Entity ent, out PlayerComponent player)
    {
        ent = _entity.CreateEntity(game);
        _players.Add(ent);
        
        player = _comp.AddComponent<PlayerComponent>(ent);
        player.PlayerToken = Guid.NewGuid().ToString();
        player.Game = game;
        
    }

    public void DetachPlayerFromGame(string playerToken)
    {
        if (!TryFindPlayer(playerToken, out var entPlayer))
            return;

        var ent = entPlayer.Value.Item1;
        
        _players.Remove(ent);
        _entity.DeleteEntity(ent);
    }

    public Entity<ControlledComponent> AssignUnitToPlayer(Entity<PlayerComponent> player, Entity entity)
    {
        var controlled = _comp.EnsureComponent<ControlledComponent>(entity);
        controlled.Player = player;
        
        return (entity, controlled);
    }
    
    private bool TryFindPlayer(string playerToken, [NotNullWhen(true)] out (Entity, PlayerComponent)? ent)
    {
        foreach (var entPlayer in _players)
        {
            if (!_comp.TryGetComponent<PlayerComponent>(entPlayer, out var player) || 
                player.PlayerToken != playerToken)
                continue;

            ent = (entPlayer, player);
            return true;
        }
        
        ent = null;
        return false;
    }
}