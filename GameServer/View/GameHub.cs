using GameServer.Model;
using Microsoft.AspNetCore.SignalR;
using GameServer.Model.Games;

namespace GameServer.View;

public class GameHub : Hub
{
    private readonly GameService _gameService;
    private readonly GamesManager _sessionManager;
    private readonly ILogger<GameHub> _logger;
    
    public GameHub(GameService gameService, GamesManager sessionManager, ILogger<GameHub> logger)
    {
        _gameService = gameService;
        _sessionManager = sessionManager;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation($"Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}