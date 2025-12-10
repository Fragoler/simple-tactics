using System.Diagnostics.CodeAnalysis;
using GameServer.Model.Components;
using GameServer.Model.Players.Components;
using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.IoC;

namespace GameServer.Model.Players.Systems;


public sealed partial class PlayersSystem : BaseSystem
{
    [Dependency] private readonly EntitySystem _entity = null!;
    [Dependency] private readonly ComponentSystem _comp = null!;

    private readonly HashSet<Entity> _players = [];

    public uint GetPlayerId(Entity entity)
    {
        if (!_comp.TryGetComponent<PlayerComponent>(entity, out var player))
            throw new ArgumentException($"Player {entity} does not have a PlayerComponent");

        return GetPlayerId((entity, player));
    }
    
    public uint GetPlayerId(Entity<PlayerComponent> ent)
    {
        return Convert.ToUInt32(ent.Ent.Game.Players.IndexOf(ent));
    }
    
    public bool TryFindPlayer(string playerToken, [NotNullWhen(true)] out Entity<PlayerComponent>? ent)
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
    
    public void CreateAttachedPlayer(Game game, out Entity ent, out PlayerComponent player)
    {
        ent = _entity.CreateEntity(game);
        _players.Add(ent);
        
        player = _comp.AddComponent<PlayerComponent>(ent);
        player.PlayerToken = Guid.NewGuid().ToString();
    }

    public void DetachPlayerFromGame(string playerToken)
    {
        if (!TryFindPlayer(playerToken, out var entPlayer))
            return;

        var ent = entPlayer.Value.Ent;
        
        _players.Remove(ent);
        _entity.DeleteEntity(ent);
    }
}