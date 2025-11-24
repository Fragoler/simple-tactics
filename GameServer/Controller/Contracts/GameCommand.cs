namespace GameServer.Controller.Contracts;

public class GameCommand
{
    public Guid GameId { get; set; }
    public Guid PlayerId { get; set; }
    public long Timestamp { get; set; }
}

public class GameCommandResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object>? ResultData { get; set; }
    public long ProcessedAt { get; set; }
}