using GameServer.Model.Action.Components;
using GameServer.Model.Action.Patterns;

namespace GameServer.Presenter.Socket.DTO;


public class ActionDto
{
    public required string Id { get; set; }
    
    public required string Name { get; set; }
    public required string Icon { get; set; }
    
    public required string TargetType { get; set; }
    public TargetFilterDto TargetFilter { get; set; }

    public required HighlightedLayerDto[] HighlightLayers { get; set; }
}


public struct TargetFilterDto
{
    public TargetFilterDto(TargetFilter filter)
    {
        Pattern = filter.Pattern.Name;
        RequiredFreeSpace = filter.RequiredFreeSpace;
        RequiredAlly = filter.RequiredAlly;
        RequiredEnemy = filter.RequiredEnemy;
        
        Range = filter.Pattern is IRangePattern ranged ? ranged.Range : null;
    }

    public string Pattern { get; set; } = "";
    public double? Range { get; set; } = 1.5;
    public bool RequiredEnemy { get; set; } = false;
    public bool RequiredAlly { get; set; } =  false;
    public bool RequiredFreeSpace { get; set; } = false;
}

public struct HighlightedLayerDto
{
    public HighlightedLayerDto(HighlightedLayer layer)
    {
        Range = layer.Pattern is IRangePattern ranged
            ? ranged.Range : null;
        
        Type = layer.Type.ToString();
        Pattern = layer.Pattern.Name;
        Relative = layer.Relative.ToString();
        Visibility = layer.Visibility.ToString();
    }
    
    public string Type { get ; set; }
    public string Pattern { get; set; }
    public string Relative { get; set; }
    public string Visibility { get; set; }
    
    public double? Range  { get; set; }
}