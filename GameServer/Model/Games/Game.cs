using System.Collections.Generic;

namespace GameServer.Model.Games;

public class Game
{
    public readonly Guid GameId = Guid.NewGuid();
    public List<Guid> Players = [];

    public Game(Guid player1Id, Guid player2Id)
    {
        Players.Add(player1Id);
        Players.Add(player2Id);
    }
}