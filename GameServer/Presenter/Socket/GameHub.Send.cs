using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Players;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;

public sealed partial class GameHub
{
    private async Task SendGameState(Game game)
    {
        var state = await SerializeGameState(game);
        await Clients.Caller.SendAsync("gameState", state);
    }

    private async Task SendPlayerId(Entity<PlayerComponent> player)
    {
        var dto = SerializePlayer(player);
        await Clients.Caller.SendAsync("playerId", dto);
    }

    private async Task SendGameActions()
    {
        var dto = SerializeActions();
        await Clients.Caller.SendAsync("gameActions", dto);
    }
}