namespace GameServer.Presenter.Socket.DTO;


public class GameStateDto
{
    public MapStateDto? Map { get; set; } = null;
    public UnitDto[] Units { get; set; } = [];
    public PlayerStateDto[] Players { get; set; } = [];
}