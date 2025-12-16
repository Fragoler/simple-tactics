using GameServer.Model.Action.Systems;
using GameServer.Model.Components;
using GameServer.Model.Effect;
using GameServer.Model.Entities;
using GameServer.Model.EventBus;
using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Phases;
using GameServer.Model.Players.Components;
using GameServer.Model.Players.Systems;
using GameServer.Presenter.Socket.DTO;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.Presenter.Socket;


public sealed partial class GameHub
    : Hub
{
    private readonly GamesSystem _games;
    private readonly PlayersSystem _players;
    private readonly ILogger<GameHub> _logger;
    
    private const string GameTokenKey = "GameToken";
    private const string PlayerTokenKey = "PlayerToken";

    public GameHub(IoCManager ioc, ILogger<GameHub> logger)
    {
        _games = ioc.Resolve<GamesSystem>();
        _players = ioc.Resolve<PlayersSystem>();
        _comp = ioc.Resolve<ComponentSystem>();
        _action = ioc.Resolve<ActionSystem>();
        _entity = ioc.Resolve<EntitySystem>();
        _phases = ioc.Resolve<PhasesSystem>();
        _event = ioc.Resolve<EventBusSystem>();
        _effects = ioc.Resolve<EffectSystem>();
        
        _logger = logger;
    }
    
    /// <summary>
    /// Client join game
    /// </summary>
    public async Task JoinGame(string gameToken, string playerToken)
    {
        try
        {
            var (game, player) = await ValidateAuth(gameToken, playerToken);
            if (game is null || player is null) return;
            
            // Accept
            Context.Items[GameTokenKey] = gameToken;
            Context.Items[PlayerTokenKey] = playerToken;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, gameToken);
            _logger.LogInformation("Player {PlayerToken} joined game {GameToken}", playerToken, gameToken);

            await Clients.Caller.SendAsync("joinedGame");
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
    public async Task RequestGameState()
    {
        var (game, player) = await ValidateAuth(); 
        if (game == null || player == null) return;
        
        try
        {
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
    public async Task RequestPlayerId()
    {
        var (game, player) = await ValidateAuth(); 
        if (game == null || player == null) return;
        
        try
        {
            await SendPlayerHimself(player.Value);
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
    public async Task RequestGameActions()
    {
        var (game, player) = await ValidateAuth(); 
        if (game == null || player == null) return;
        
        try
        {
            await SendGameActions();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting game actions");
            await Clients.Caller.SendAsync("error", ex.Message);
        }
    }

    public async Task RequestTurnEnd(ScheduledActionDto[] actionsDto)
    {
        var (game, player) = await ValidateAuth();
        if (game == null || player == null) return;
        
        try
        {
            await HandlePlayerSchedule(player.Value, actionsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting turn end");
        }
    }
    
    
    private async Task<(Game? game, Entity<PlayerComponent>? player)> ValidateAuth()
    {
        if (!Context.Items.TryGetValue(GameTokenKey, out var gameTokenObj) ||
            !Context.Items.TryGetValue(PlayerTokenKey, out var playerTokenObj))
        {
            await Clients.Caller.SendAsync("error", "Not authenticated. Call JoinGame first.");
            return (null, null);
        }

        var gameToken = gameTokenObj as string ?? string.Empty;
        var playerToken = playerTokenObj as string ?? string.Empty;

        return await ValidateAuth(gameToken, playerToken);
    }
    
    private async Task<(Game? game, Entity<PlayerComponent>? player)> ValidateAuth(string gameToken, string playerToken)
    {
        var game = _games.GetGame(gameToken);
        if (game == null)
        {
            await Clients.Caller.SendAsync("error", "Game not found");
            return (null, null);
        }

        if (!_players.TryFindPlayer(playerToken, out var player))
        {
            await Clients.Caller.SendAsync("error", "Player not found");
            return (null, null);
        }

        if (player.Value.Ent.Game != game)
        {
            await Clients.Caller.SendAsync("error", "Invalid player token");
            return (null, null);
        }
        
        return (game, player);
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