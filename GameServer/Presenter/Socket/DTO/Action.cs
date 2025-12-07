using GameServer.Model.Action.Components;

namespace GameServer.Presenter.Socket.DTO;


public class ActionDto
{
    public required string Id { get; set; }
    
    public required string Name { get; set; }
    public required string Icon { get; set; }
    
    public ActionTargetType TargetType { get; set; }
    public TargetFilter TargetFilter { get; set; }

    public required HighlightedLayer[] HighlightLayers { get; set; }
}
