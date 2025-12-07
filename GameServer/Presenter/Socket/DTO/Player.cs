namespace GameServer.Presenter.Socket.DTO;

public class PlayerStateDto
{
    public uint PlayerId { get; set; }
    public string? PlayerName { get; set; } = "";
    public bool IsReady { get; set; }
}