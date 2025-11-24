using GameServer.Controller.Contracts;
using GameServer.Model.Games;

namespace GameServer.Model;

public class GameService
{
	private readonly GamesManager _sessionManager;
	private readonly ILogger<GameService> _logger;

	public GameService(GamesManager sessionManager, ILogger<GameService> logger)
	{
		_sessionManager = sessionManager;
		_logger = logger;
	}
	
}