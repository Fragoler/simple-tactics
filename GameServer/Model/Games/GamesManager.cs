namespace GameServer.Model.Games;

public class GamesManager
{
    private readonly Dictionary<Guid, Game> _games = new();
    private readonly object _lock = new();

    public Game? CreateGame(Guid player1Id, Guid player2Id)
    {
        var gameState = new Game(player1Id, player2Id);
        
        lock (_lock)
        {
            try
            {
                _games[gameState.GameId] = gameState;
                return gameState;
            }
            finally {}
        }
    }

    public Game? GetGame(Guid gameId)
    {
        lock (_lock)
        {
            try
            {
                return _games.GetValueOrDefault(gameId);
            }
            finally {}
        }
    }

    public void DeleteGame(Guid gameId)
    {
        lock (_lock)
        {
            try
            {
                _games.Remove(gameId);
            }
            finally {}
        }
    }
}