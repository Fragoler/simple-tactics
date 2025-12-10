using GameServer.Model.Entities;
using GameServer.Model.Games;
using GameServer.Model.Players.Components;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;

public sealed partial class GameHub
{
    private async Task SendGameState(Game game)
    {
        var state = await SerializeGameState(game);
        await Clients.Caller.SendAsync("gameState", state);
    }
    
    private async Task SendPlayersStateToAll(Game game)
    {
        var dto = SerializePlayers(game);
        await Clients
            .Group((string) Context.Items[GameTokenKey]!)
            .SendAsync("playersState", dto);
    }
    private async Task SendPlayersState(Entity<PlayerComponent> player)
    {
        var dto = SerializePlayers(player.Ent.Game);
        await Clients.Caller.SendAsync("playersState", dto);
    }

    private async Task SendPlayerHimself(Entity<PlayerComponent> player)
    {
        var dto = SerializePlayer(player);
        await Clients.Caller.SendAsync("yourPlayer", dto);
    }

    private async Task SendGameActions()
    {
        var dto = SerializeActions();
        await Clients.Caller.SendAsync("gameActions", dto);
    }
}