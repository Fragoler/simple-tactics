using GameServer.Model.Games;
using GameServer.Model.IoC;
using GameServer.Model.Map;
using GameServer.Presenter.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.Presenter.API;


[ApiController]
[Route("api/game")]
public class GamesController(IoCManager ioc, ILogger<GamesController> logger) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly GamesSystem _games = ioc.Resolve<GamesSystem>();
    private readonly MapSystem _map = ioc.Resolve<MapSystem>();

    
    [HttpPost("create")]
    public IActionResult CreateGame()
    {
        var gameToken = _games.CreateGame();
        
        _logger.LogInformation("Game created with token: {GameToken}", gameToken);
        
        if (!_games.TryAddPlayer(gameToken, out var playerToken1) ||
            !_games.TryAddPlayer(gameToken, out var playerToken2))
        {
            _logger.LogError("Failed to add players to game {GameToken}", gameToken);
            _games.DeleteGame(gameToken);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        _logger.LogDebug("Players added: {Token1}, {Token2}", playerToken1, playerToken2);
        
        if (_games.GetGame(gameToken) is not { } game) 
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        _map.LoadMapForGame(game);
        _logger.LogInformation("Loaded map for game: {GameToken}", gameToken);
        
        var dto = GameToDto(game);
        _logger.LogDebug("Returning DTO: GameToken={GameToken}, PlayerCount={Count}", 
            dto.GameToken, dto.PlayerTokens.Length);
        return Ok(dto);

    }

    [HttpGet("list")]
    public IActionResult ListGames()
    {
        var games = new GamesDto
        {
            GameTokens = _games.GetGames().Select(GameToDto).ToArray()
        };
        return Ok(new { games });
    }
    
    [HttpGet("get")]
    public IActionResult GetGame([FromQuery] string token)
    {
        if (_games.GetGame(token) is {  } game)
            return Ok(GameToDto(game));
        return StatusCode(StatusCodes.Status400BadRequest);
    }

    [HttpPost("remove")]
    public IActionResult RemoveGame([FromQuery] string token)
    {
        if (_games.GetGame(token) is null)
            return StatusCode(StatusCodes.Status400BadRequest);
        
        _games.DeleteGame(token);
        return Ok();
    }

    private GameTokensDto GameToDto(Game game)
    {
        return new GameTokensDto
        {
            GameToken = game.Token,
            PlayerTokens = game.Players.Select(p => p.Component.PlayerToken).ToArray()
        };
    }
}