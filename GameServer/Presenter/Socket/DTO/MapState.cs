namespace GameServer.Presenter.Socket.DTO;


public class MapStateDto
{
    public required string MapName { get; set; } = "";
    public required uint Width { get; set; }
    public required uint Height { get; set; }
    
    public required uint[][] Terrain { get; set; }
}