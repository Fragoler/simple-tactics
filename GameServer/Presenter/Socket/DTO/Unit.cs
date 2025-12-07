using GameServer.Model.Sprite;

namespace GameServer.Presenter.Socket.DTO;


public class UnitDto
{
    public ulong UnitId { get; set; }
    public uint? PlayerId { get; set; }
    
    public required PositionDto Coords { get; set; }
    public required SpriteType Sprite { get; set; }
    
    public required string[] ActionIds { get; set; }
    
    public uint? CurHealth { get; set; }
    public uint? MaxHealth { get; set; }
    
}