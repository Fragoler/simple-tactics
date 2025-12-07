using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Players;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub(IoCManager ioc, ILogger<GameHub> logger)
    : Hub
{
    private readonly GamesSystem _games = ioc.Resolve<GamesSystem>();
    private readonly PlayersSystem _players = ioc.Resolve<PlayersSystem>();

    private readonly ILogger<GameHub> _logger = logger;

    
    /// <summary>
    /// Client join game
    /// </summary>
    public async Task JoinGame(string gameToken, string playerToken)
    {
        try
        {
            var game = _games.GetGame(gameToken);
            if (game == null)
            {
                await Clients.Caller.SendAsync("error", "Game not found");
                return;
            }
            
            var playerExists = game.Players.Any(p => p.Component.PlayerToken == playerToken);
            if (!playerExists)
            {
                await Clients.Caller.SendAsync("error", "Player not found in game");
                return;
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, gameToken);
            _logger.LogInformation("Player {PlayerToken} joined game {GameToken}", playerToken, gameToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error joining game");
            await Clients.Caller.SendAsync("error", ex.Message);
        }
    }

    /// <summary>
    /// Client request all game state
    /// </summary>
    public async Task RequestGameState(string gameToken, string playerToken)
    {
        _logger.LogInformation("Requesting game state {GameToken} by player {playerToken} ", gameToken, playerToken);
        
        try
        {
            var game = _games.GetGame(gameToken);
            if (game == null)
            {
                await Clients.Caller.SendAsync("error", "Game not found");
                return;
            }

            await SendGameState(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting game state");
            await Clients.Caller.SendAsync("error", ex.Message);
        }
    }
    
    
    /// <summary>
    /// Client request its id
    /// </summary>
    public async Task RequestPlayerId(string gameToken, string playerToken)
    {
        _logger.LogInformation("Requested player id  {PlayerToken} for game {GameToken}", playerToken, gameToken);
        
        try
        {
            if (!_players.TryFindPlayer(playerToken, out var ent))
            {
                await Clients.Caller.SendAsync("error", "Player not found");
                return;
            }

            await SendPlayerId(ent.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting player id");
            await Clients.Caller.SendAsync("error", ex.Message);
        }
    }
    
    
    /// <summary>
    /// Client request all game actions
    /// </summary>
    public async Task RequestGameActions(string gameToken, string playerToken)
    {
        _logger.LogInformation("Requested game actions {PlayerToken} for game {GameToken}", playerToken, gameToken);
        
        try
        {
            if (!_players.TryFindPlayer(playerToken, out var ent))
            {
                await Clients.Caller.SendAsync("error", "Player not found");
                return;
            }

            await SendGameActions();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting game actions");
            await Clients.Caller.SendAsync("error", ex.Message);
        }
    }
    

    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}