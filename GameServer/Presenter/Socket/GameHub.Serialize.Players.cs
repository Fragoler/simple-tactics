using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Players.Components;
using GameServer.Presenter.Socket.DTO;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
{
    private PlayerStateDto[] SerializePlayers(Game game)
    {
        return game.Players.Select(SerializePlayer).ToArray();
    }

    private PlayerStateDto SerializePlayer(Entity<PlayerComponent> ent)
    {
        var id = _players.GetPlayerId(ent);
        
        return new PlayerStateDto 
        {
            PlayerId = id,
            PlayerName = $"Player {id + 1}",
            IsReady = ent.Component.IsReady,
        };
    }
}